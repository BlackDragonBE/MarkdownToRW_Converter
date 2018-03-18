using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using DragonMarkdown;
using MarkdownToRW.Properties;
using nQuant;
using WordPressSharp;
using WordPressSharp.Models;

namespace MarkdownToRW
{
    public partial class ImageUploadWindow : Form
    {
        private readonly List<int> _imageIdList = new List<int>();
        private bool _errorInUpload;
        private long _totalFileSizes;

        public ImageUploadWindow(ImageUploadData imageUploadData)
        {
            InitializeComponent();
            ImageUploadData = imageUploadData;
            ServicePointManager.ServerCertificateValidationCallback = MonoHelper.Validator;
            CheckConnectionToRW();

            FillImageNameList();

            if (Settings.Default.CredentialsSaved)
            {
                txtUsername.Text = Settings.Default.Username;
                txtPassword.Text = Settings.Default.Password;
            }

            if (MonoHelper.IsRunningOnMono)
            {
                btnMacPasteUsername.Visible = true;
                btnMacPastePassword.Visible = true;
            }

            SetLoginControlsEnabledState(true);
        }

        public ImageUploadData ImageUploadData { get; }

        private void CheckConnectionToRW()
        {
            WordPressConnector.TestConnection();
        }

        private void FillImageNameList()
        {
            listPreviews.Items.Clear();

            foreach (string imagePath in ImageUploadData.FullImagePaths)
            {
                long currentSize = new FileInfo(imagePath).Length;
                listPreviews.Items.Add(imagePath + "| ( " + currentSize / 1024 + " kB )");
                _totalFileSizes += currentSize;
            }

            listPreviews.Items.Add("Total size: " + _totalFileSizes / 1024 + " kB. (" + _totalFileSizes / 1024 / 1024 +
                                   " MB)");
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            _errorInUpload = false;

            if (MessageBox.Show(
                    "Make sure this is the first time you upload these images and double check that the paths are correct before continuing.\n" +
                    "You may have to manually delete uploaded images if anything goes wrong. \n\n" +
                    "Do you want to upload the images and update both your markdown and html?",
                    "Are you sure you want to start the upload?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                UploadImages();

                if (!_errorInUpload)
                {
                    UpdateMarkdownAndHtml();
                    MessageBox.Show(
                        "Succesfully uploaded " + ImageUploadData.FullImagePaths.Count +
                        " images to the RW WordPress!\nYour markdown and the html preview have been updated with the image URLs. The HTML is fully ready to copy to WordPress!",
                        "Upload complete!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void UploadImages()
        {
            lblStatus.Text = "Starting Upload...";
            _imageIdList.Clear();
            progressUpload.Maximum = ImageUploadData.FullImagePaths.Count;

            try
            {
                foreach (string path in ImageUploadData.FullImagePaths)
                {
                    lblStatus.Text = "Uploading " + path + "...";
                    lblStatus.Refresh();
                    UploadResult result = null;

                    if (chkOptimizeImages.Checked && path.ToLower().EndsWith("png"))
                    {
                        var quantisizer = new WuQuantizer();
                        var bitmap = new Bitmap(path);

                        using (var quantized = quantisizer.QuantizeImage(bitmap))
                        {
                            quantized.Save(Application.StartupPath + "/" + Path.GetFileName(path));
                        }

                        result = WordPressConnector.UploadFile(Application.StartupPath + "/" + Path.GetFileName(path));
                    }
                    else
                    {
                        result = WordPressConnector.UploadFile(path);
                    }

                    if (result != null)
                    {
                        ImageUploadData.ImageUrls.Add(result.Url);
                        _imageIdList.Add(Convert.ToInt32(result.Id));
                    }
                    else
                    {
                        _errorInUpload = true;
                        MessageBox.Show(
                            "Something went wrong while uploading. Press OK to attempt rollback, make sure you're connected to the internet and can access the RW WordPress before continuing.",
                            "Error while uploading");
                        TryToDeleteImages();
                    }

                    progressUpload.Value++;
                }

                if (chkOptimizeImages.Checked)
                {
                    //Remove optimized images
                    foreach (string path in ImageUploadData.FullImagePaths)
                    {
                        if (File.Exists(Application.StartupPath + "/" + Path.GetFileName(path)))
                        {
                            File.Delete(Application.StartupPath + "/" + Path.GetFileName(path));
                        }
                    }
                }

                lblStatus.Text = "Uploading complete!";
            }
            catch (Exception e)
            {
                _errorInUpload = true;
                Console.WriteLine(e);
                TryToDeleteImages();
            }
        }

        private void UpdateMarkdownAndHtml()
        {
            if (ImageUploadData.FullImagePaths.Count != ImageUploadData.ImageUrls.Count)
            {
                int count = ImageUploadData.FullImagePaths.Count - ImageUploadData.ImageUrls.Count;
                TryToDeleteImages();
                MessageBox.Show(
                    "There is a mismatch between requested image uploads and actual uploads!\nLog in to WordPress and delete the uploaded media or fix the html manually.\nThere are " +
                    count + " images that weren't uploaded.", "Something went wrong!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            string markdown = ImageUploadData.OldMarkdown;

            for (int i = 0; i < ImageUploadData.ImageUrls.Count; i++)
            {
                string url = ImageUploadData.ImageUrls[i];
                markdown = markdown.Replace(ImageUploadData.LocalImagePaths[i], url);
            }

            if (chkUpdateMarkdown.Checked)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(ImageUploadData.MarkdownPath))
                    {
                        sw.Write(markdown);
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "Failed to save file: " + ImageUploadData.MarkdownPath +
                        "\nBe sure to copy the markdown text from the main window if you don't want to lose it.",
                        "Markdown not saved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            ImageUploadData.NewMarkdown = markdown;
        }

        private void TryToDeleteImages()
        {
            lblStatus.Text = "Rolling back...";

            WordPressSiteConfig config = new WordPressSiteConfig();
            config.Username = txtUsername.Text;
            config.Password = txtPassword.Text;
            config.BaseUrl = "https://www.raywenderlich.com";

            try
            {
                // Something went wrong, delete all uploaded images
                for (var i = 0; i < _imageIdList.Count; i++)
                {
                    int id = _imageIdList[i];
                    bool deleted = WordPressConnector.Delete(id);
                    progressUpload.Value--;

                    if (!deleted)
                    {
                        MessageBox.Show("Couldn't delete image: " + ImageUploadData.ImageUrls[i]);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Image rollback failed. Please delete the remaining images manually.");
                return;
            }

            MessageBox.Show("Rollback succesfull!");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            WordPressConnector.InitializeWordPress(txtUsername.Text, txtPassword.Text);

            lblStatus.Text = "Verifying...";
            SetLoginControlsEnabledState(false);

            User user = WordPressConnector.GetUserProfile();

            if (user != null)
            {
                MessageBox.Show("Thanks " + user.FirstName + "!" + "\nYou're ready to upload.");
                btnUpload.Enabled = true;
            }
            else
            {
                MessageBox.Show("Connection failed",
                    "Can't connect to RW Wordpress!");
                SetLoginControlsEnabledState(true);
                lblStatus.Text = "Can't connect to RW WordPress, please try again.";
                return;
            }

            if (chkSaveCredentials.Checked)
            {
                // Save credentials
                Settings.Default.CredentialsSaved = true;
                Settings.Default.Username = txtUsername.Text;
                Settings.Default.Password = txtPassword.Text;
                Settings.Default.Save();
            }
        }

        private void SetLoginControlsEnabledState(bool enabled)
        {
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            btnMacPasteUsername.Enabled = enabled;
            btnMacPastePassword.Enabled = enabled;
            Refresh();
        }

        private void listPreviews_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listPreviews.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches && index != listPreviews.Items.Count - 1)
            {
                Process.Start(listPreviews.Items[index].ToString().Split('|')[0]);
            }
        }

        private void ImageUploadWindow_Load(object sender, EventArgs e)
        {
        }

        private void btnMacPasteUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Text = MonoHelper.PasteFromMacClipboard();
        }

        private void btnMacPastePassword_Click(object sender, EventArgs e)
        {
            txtPassword.Text = MonoHelper.PasteFromMacClipboard();
        }
    }
}