using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using DragonMarkdown;
using DragonMarkdown.DragonWordPressXml.Responses;
using DragonMarkdown.Utility;

namespace MarkdownToRWGUI.Models
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _allowInput = true;

        private string _htmlPath;
        private string _htmlPreviewPath;
        private string _htmlText;

        private bool _markdownLoaded;
        private string _markdownPath;
        private string _markdownText;

        private bool _onlyHtml;

        private string _password;
        private string _passwordOverlay;
        private bool _saveOutputToHtml;
        private string _status;
        private string _username;
        private bool _rememberCredentials;

        private int _progressValue;
        private int _progressMin;
        private int _progressMax;

        public Window ThisWindow;

        public bool AllowInput
        {
            get => _allowInput;
            set
            {
                if (value != _allowInput)
                {
                    _allowInput = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool MarkdownLoaded
        {
            get => _markdownLoaded;
            set
            {
                if (value != _markdownLoaded)
                {
                    _markdownLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool OnlyHtml
        {
            get => _onlyHtml;
            set
            {
                if (value != _onlyHtml)
                {
                    _onlyHtml = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MarkdownText
        {
            get => _markdownText;
            set
            {
                if (value != _markdownText)
                {
                    _markdownText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HtmlText
        {
            get => _htmlText;
            set
            {
                if (value != _htmlText)
                {
                    _htmlText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SaveOutputToHtml
        {
            get => _saveOutputToHtml;
            set
            {
                if (value != _saveOutputToHtml)
                {
                    _saveOutputToHtml = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    PasswordOverlay = DragonUtil.GetPasswordChars(_password.Length, '●');
                    OnPropertyChanged();
                }
            }
        }

        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                if (value != _progressValue)
                {
                    _progressValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ProgressMin
        {
            get => _progressMin;
            set
            {
                if (value != _progressMin)
                {
                    _progressMin = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ProgressMax
        {
            get => _progressMax;
            set
            {
                if (value != _progressMax)
                {
                    _progressMax = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool RememberCredentials
        {
            get => _rememberCredentials;
            set
            {
                if (value != _rememberCredentials)
                {
                    _rememberCredentials = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordOverlay
        {
            get => _passwordOverlay;
            set
            {
                if (value != _passwordOverlay)
                {
                    _passwordOverlay = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void Convert()
        {
            _markdownPath = null;
            _htmlPath = null;

            string path = await ChooseFile();

            if (path != null)
            {
                if (File.Exists(path))
                {
                    _markdownPath = path;

                    using (StreamReader sr = new StreamReader(path))
                    {
                        MarkdownText = sr.ReadToEnd().Replace("\t", "  ");
                        HtmlText = Converter.ConvertMarkdownStringToHtml(MarkdownText);

                        if (SaveOutputToHtml)
                        {
                            _htmlPath = DragonUtil.GetFullPathWithoutExtension(path) + ".html";
                            Converter.ConvertMarkdownFileToHtmlFile(path, _htmlPath);
                        }

                        Status = "Converted markdown to HTML!";
                        MarkdownLoaded = true;
                    }
                }
            }
            else
            {
                Status = "No valid markdown chosen!";
            }
        }

        private async Task<string> ChooseFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.AllowMultiple = false;

            FileDialogFilter markdownFilter = new FileDialogFilter();
            markdownFilter.Extensions.Add("md");
            markdownFilter.Extensions.Add("markdown");
            markdownFilter.Extensions.Add("mdown");
            markdownFilter.Extensions.Add("mkdn");
            markdownFilter.Extensions.Add("mkd");
            markdownFilter.Extensions.Add("mdwn");
            markdownFilter.Extensions.Add("mdtxt");
            markdownFilter.Extensions.Add("mdtext");
            markdownFilter.Extensions.Add("text");
            markdownFilter.Extensions.Add("txt");
            markdownFilter.Extensions.Add("rmd");
            markdownFilter.Name = "Markdown Files";

            FileDialogFilter allFilter = new FileDialogFilter();
            allFilter.Extensions.Add("*");
            allFilter.Name = "All Files";

            openDialog.Filters.Add(markdownFilter);
            openDialog.Filters.Add(allFilter);
            string[] resultString = await openDialog.ShowAsync(Window.OpenWindows.FirstOrDefault());

            if (resultString != null && resultString.Length > 0)
            {
                return resultString[0];
            }

            return null;
        }

        public void ShowPreview()
        {
            Status = "Generating preview and opening...";

            string folderPath = Path.GetDirectoryName(_markdownPath);
            _htmlPreviewPath = folderPath + "/tmp.html";

            PreviewCreator.CreateHtmlPreviewFileFromMarkdown(MarkdownText, _htmlPreviewPath);

            DragonUtil.OpenFileInDefaultApplication(_htmlPreviewPath);

            Status = "Created preview.";

            WaitAndDeletePreviewAsync();
        }

        private async Task WaitAndDeletePreviewAsync()
        {
            await Task.Delay(10000);

            if (File.Exists(_htmlPreviewPath))
            {
                File.Delete(_htmlPreviewPath);
                Status = "Deleted preview: " + _htmlPreviewPath;
                _htmlPreviewPath = null;
            }
        }

        public async void UploadImages()
        {
            //Status = "Image uploading isn't implemented yet :(";
            //return;

            if (Username == "" || Password == "" || Username == null || Password == null)
            {
                Status = "Credentials not filled in correctly!";
                return;
            }

            if (RememberCredentials)
            {
                 SettingsManager.SaveSettings(new Settings(){Username = Username, Password = Password, ShouldLoadCredentials = true});
            }

            ProgressValue = 0;
            ProgressMax = 3;

            AllowInput = false;
            Status = "Testing connection...";
            await Task.Delay(25);

            if (WordPressConnector.CanConnectToRW())
            {
                Status = "Initial connection to RW website OK. Connecting to WordPress...";
                ProgressValue++;
                await Task.Delay(25);

                WordPressConnector.InitializeWordPress(Username, Password);

                GetProfileResponse profile = WordPressConnector.GetUserProfile();

                if (profile != null)
                {
                    Status = "Thanks " + profile.FirstName + "! Gathering images...";
                    ProgressValue++;
                    DoUpload();
                }
                else
                {
                    Status = "Couldn't log in. Please check your credentials.";
                    AllowInput = true;
                }
            }
            else
            {
                Status = "Connection failed. Can't connect to RW.";
                AllowInput = true;
            }
        }

        private async void DoUpload()
        {
            await Task.Delay(1000);
            var links = Converter.FindAllImageLinksInHtml(HtmlText, Path.GetDirectoryName(_markdownPath));

            if (links.Count == 0)
            {
                Status = "No images found to upload.";
                return;
            }

            Status = "Images loaded. Starting upload, please don't close this window while uploading.";
            ProgressValue++;
            await Task.Delay(1000);

            List<string> fullImagePaths = new List<string>();
            List<string> localImagePaths = new List<string>();

            foreach (ImageLinkData link in links)
            {
                fullImagePaths.Add(link.FullImagePath);
                localImagePaths.Add(link.LocalImagePath);
            }

            Console.WriteLine("");
            Console.WriteLine(fullImagePaths.Count + " image paths found:");

            foreach (string path in fullImagePaths)
            {
                Console.WriteLine(path + " (" + new FileInfo(path).Length / 1024 + " kb)");
            }

            List<string> imageUrls = new List<string>();
            List<string> imageIDs = new List<string>();
            ProgressValue = 0;
            ProgressMax = fullImagePaths.Count;

            // Upload images
            for (var i = 0; i < fullImagePaths.Count; i++)
            {
                string path = fullImagePaths[i];

                Status = "Uploading: " + " (" + (i + 1) + "/" + fullImagePaths.Count + ") " + path + "...";
                await Task.Delay(25);

                var result = WordPressConnector.UploadFile(path);

                if (result != null)
                {
                    imageUrls.Add(result.FileResponseStruct.Url);
                    imageIDs.Add(result.FileResponseStruct.Id.ToString());
                    ProgressValue++;
                    await Task.Delay(25);
                }
                else
                {
                    Status = "Image upload failed! Starting rollback...";
                    await Task.Delay(25);

                    foreach (string iD in imageIDs)
                    {
                        var deleted = WordPressConnector.Delete(System.Convert.ToInt32(iD));
                        if (deleted)
                        {
                            Status = "Deleted file with id " + iD;
                        }
                        else
                        {
                            Status = "Failed to delete file with id " + iD;
                        }
                        await Task.Delay(25);
                    }
                }
            }

            // Update markdown & html
            Console.WriteLine("Starting link replacer...");
            MarkdownText = Converter.ReplaceLocalImageLinksWithUrls(_markdownPath, _htmlPath, OnlyHtml, MarkdownText, localImagePaths, imageUrls);
            HtmlText = Converter.ConvertMarkdownStringToHtml(MarkdownText);

            Status = "Upload & replacement complete!";
            AllowInput = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}