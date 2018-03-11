using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using MarkdownToRW.Properties;
using nQuant;
using WordPressSharp;
using WordPressSharp.Models;

namespace MarkdownToRW
{
    public partial class ImageUploadWindow : Form
    {
        public ImageUploadWindow(ImageUploadData imageUploadData)
        {
            InitializeComponent();
            ImageUploadData = imageUploadData;

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
                chkOptimizeImages.Checked = false;
                chkOptimizeImages.Enabled = false;
            }
        }

        private List<int> imageIdList = new List<int>();
        private long totalFileSizes = 0;
        private long fileSizeSaved = 0;

        public ImageUploadData ImageUploadData { get; }

        private void FillImageNameList()
        {
            listPreviews.Items.Clear();

            foreach (string imagePath in ImageUploadData.FullImagePaths)
            {
                long currentSize = new FileInfo(imagePath).Length;
                listPreviews.Items.Add(imagePath + "| ( " + currentSize / 1024 + " kB )");
                totalFileSizes += currentSize;
            }

            listPreviews.Items.Add("Total size: " + totalFileSizes / 1024 + " kB. (" + totalFileSizes / 1024 / 1024 + " MB)");
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Make sure this is the first time you upload these images and double check that the paths are correct before continuing.\n" +
                    "You may have to manually delete uploaded images if anything goes wrong. \n\n" +
                    "Do you want to upload the images and update both your markdown and html?",
                    "Are you sure you want to start the upload?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                UploadImages();
                UpdateMarkdownAndHtml();
                MessageBox.Show(
                    "Succesfully uploaded " + ImageUploadData.FullImagePaths.Count +
                    " images to the RW WordPress!\nYour markdown and the html preview have been updated with the image URLs. The HTML is fully ready to copy to WordPress!",
                    "Upload complete!");
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void UploadImages()
        {
            

            //Process.Start("mozroots", "--import --quiet"); // Import certificates (workaround for mono having no certs by default)
            lblStatus.Text = "Starting Upload...";
            imageIdList.Clear();

            WordPressSiteConfig config = new WordPressSiteConfig
            {
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                BaseUrl = "https://www.raywenderlich.com"
            };

            progressUpload.Maximum = ImageUploadData.FullImagePaths.Count;

            try
            {
                using (WordPressClient client = new WordPressClient(config))
                {
                    foreach (string path in ImageUploadData.FullImagePaths)
                    {
                        lblStatus.Text = "Uploading " + path + "...";
                        lblStatus.Refresh();
                        string mimeType = "image/" + Path.GetExtension(path).ToLower();

                        Data image = null;

                        if (chkOptimizeImages.Checked && path.ToLower().EndsWith("png"))
                        {
                            var quantisizer = new WuQuantizer();
                            var bitmap = new Bitmap(path);

                            using (var quantized = quantisizer.QuantizeImage(bitmap))
                            {
                                quantized.Save(Path.GetFileName(path));
                            }

                            fileSizeSaved += (new FileInfo(path).Length - new FileInfo(Path.GetFileName(path)).Length);

                            image = Data.CreateFromFilePath(Path.GetFileName(path), mimeType);
                        }
                        else
                        {
                            image = Data.CreateFromFilePath(path, mimeType);
                        }


                        var result = client.UploadFile(image);
                        ImageUploadData.ImageUrls.Add(result.Url);
                        imageIdList.Add(Convert.ToInt32(result.Id));
                        progressUpload.Value++;
                    }
                }

                if (chkOptimizeImages.Checked)
                {
                    //Remove optimized images
                    foreach (string path in ImageUploadData.FullImagePaths)
                    {
                        if (File.Exists(Path.GetFileName(path)))
                        {
                            File.Delete(Path.GetFileName(path));
                        }
                    }

                    MessageBox.Show(
                        "You have uploaded " + (fileSizeSaved / 1024) +
                        " kB less because of optimization!", "Optimization Results");
                }

                lblStatus.Text = "Uploading complete!";
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while uploading. Press OK to attempt rollback, make sure you're connected to the internet and can access the RW WordPress before continuing.\n" + e.Source + " " + e.Message,
                    "Error while uploading");
                TryToDeleteImages();

                DialogResult = DialogResult.Abort;
                Close();
                return;
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
            //config.Username = "ekerckhove";
            //config.Password = "AjRSs8HZ";
            config.Username = txtUsername.Text;
            config.Password = txtPassword.Text;
            config.BaseUrl = "https://www.raywenderlich.com";

            try
            {
                using (WordPressClient client = new WordPressClient(config))
                {
                    // Something went wrong, delete all uploaded images
                    for (var i = 0; i < imageIdList.Count; i++)
                    {
                        int id = imageIdList[i];
                        bool deleted = client.DeletePost(id);
                        progressUpload.Value--;

                        if (!deleted)
                        {
                            MessageBox.Show("Couldn't delete image: " + ImageUploadData.ImageUrls[i]);
                        }
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
            lblStatus.Text = "Verifying...";
            SetLoginControlsEnabledState(false);

            WordPressSiteConfig config = new WordPressSiteConfig();
            //config.Username = "ekerckhove";
            //config.Password = "AjRSs8HZ";
            config.Username = txtUsername.Text;
            config.Password = txtPassword.Text;
            config.BaseUrl = "https://www.raywenderlich.com";

            try
            {
                using (WordPressClient client = new WordPressClient(config))
                {
                    string name = client.GetProfile().FirstName;
                    MessageBox.Show("Thanks " + name + "!" + "\nYou're ready to upload.");
                    btnUpload.Enabled = true;
                    SetLoginControlsEnabledState(false);
                    lblStatus.Text = "Ready to upload";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Incorrect credentials or other exception was thrown:\n" + exception.Message,
                    "Can't connect to RW Wordpress!");
                SetLoginControlsEnabledState(true);
                lblStatus.Text = "Incorrect credentials, please try again.";
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