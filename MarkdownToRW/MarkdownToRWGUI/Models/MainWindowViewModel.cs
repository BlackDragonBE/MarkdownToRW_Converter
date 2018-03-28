using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using DragonMarkdown;
using DragonMarkdown.ContentScan;
using DragonMarkdown.DragonConverter;
using DragonMarkdown.DragonWordPressXml.Responses;
using DragonMarkdown.Utility;

namespace MarkdownToRWGUI.Models
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _allowInput = true;
        private bool _firstImageRight;
        private string _htmlPath;
        private string _htmlPreviewPath;
        private string _htmlText;
        private bool _markdownLoaded;
        private string _markdownPath;
        private string _markdownText;
        private bool _newUpdate;
        private bool _onlyHtml;
        private string _password;
        private string _passwordOverlay;
        private int _progressMax;
        private int _progressMin;
        private int _progressValue;
        private bool _rememberCredentials;
        private bool _replaceImageAlts;
        private bool _saveConverterSettings;
        private bool _saveOutputToHtml;
        private string _status;
        private string _username;

        public string ActualPassword;
        public Settings Settings;
        public Window ThisWindow;
        public TextBox TxtPassword;
        public string UpdateDownloadUrl;

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

        public bool FirstImageRight
        {
            get => _firstImageRight;
            set
            {
                if (value != _firstImageRight)
                {
                    _firstImageRight = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SaveConverterSettings
        {
            get => _saveConverterSettings;
            set
            {
                if (value != _saveConverterSettings)
                {
                    _saveConverterSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool NewUpdate
        {
            get => _newUpdate;
            set
            {
                if (value != _newUpdate)
                {
                    _newUpdate = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ReplaceImageAlts
        {
            get => _replaceImageAlts;
            set
            {
                if (value != _replaceImageAlts)
                {
                    _replaceImageAlts = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ConverterOptions GetConverterOptions()
        {
            return new ConverterOptions
            {
                FirstImageIsAlignedRight = FirstImageRight,
                ReplaceImageWithAltWithCaption = ReplaceImageAlts
            };
        }

        public async void Convert()
        {
            SaveSettings();
            string path = await ChooseFile();

            ConverterOptions options = GetConverterOptions();

            if (path != null)
            {
                if (File.Exists(path))
                {
                    _markdownPath = path;
                    _htmlPath = null;

                    using (StreamReader sr = new StreamReader(path))
                    {
                        MarkdownText = sr.ReadToEnd().Replace("\t", "  ");

                        Console.Write(ContentScanner.ParseScanrResults(ContentScanner.ScanMarkdown(MarkdownText)));


                        HtmlText = Converter.ConvertMarkdownStringToHtml(MarkdownText, options);

                        if (SaveOutputToHtml)
                        {
                            _htmlPath = DragonUtil.GetFullPathWithoutExtension(path) + ".html";
                            Converter.ConvertMarkdownFileToHtmlFile(path, _htmlPath, options);
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

        public void DownloadUpdate()
        {
            DragonUtil.OpenFileInDefaultApplication(UpdateDownloadUrl);
        }

        public async void ShowPreview()
        {
            Status = "Generating preview and opening...";

            string folderPath = Path.GetDirectoryName(_markdownPath);
            _htmlPreviewPath = folderPath + "/tmp.html";

            PreviewCreator.CreateHtmlPreviewFileFromMarkdown(MarkdownText, _htmlPreviewPath);

            DragonUtil.OpenFileInDefaultApplication(_htmlPreviewPath);

            Status = "Created preview.";

            await WaitAndDeletePreviewAsync();
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

        public void OpenHemingway()
        {
            DragonUtil.OpenFileInDefaultApplication("http://www.hemingwayapp.com/");
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

            SaveSettings();

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
                    OnActionFinished();
                }
            }
            else
            {
                Status = "Connection failed. Can't connect to RW.";
                OnActionFinished();
            }
        }

        private void SaveSettings()
        {
            if (RememberCredentials)
            {
                Settings.Username = Username;
                Settings.Password = Password;
                Settings.ShouldLoadCredentials = true;
            }
            else
            {
                Settings.Username = "";
                Settings.Password = "";
                Settings.ShouldLoadCredentials = false;
            }

            Settings.RememberConverterSettings = SaveConverterSettings;

            if (SaveConverterSettings)
            {
                Settings.ConverterOptions.FirstImageIsAlignedRight = FirstImageRight;
                Settings.ConverterOptions.ReplaceImageWithAltWithCaption = ReplaceImageAlts;
                Settings.OutputToHtml = SaveOutputToHtml;
            }
            else
            {
                Settings.ConverterOptions.FirstImageIsAlignedRight = true;
                Settings.ConverterOptions.ReplaceImageWithAltWithCaption = true;
                Settings.OutputToHtml = false;
            }

            SettingsManager.SaveSettings(Settings);
        }

        private async void DoUpload()
        {
            await Task.Delay(1000);
            var links = Converter.FindAllImageLinksInHtml(HtmlText, Path.GetDirectoryName(_markdownPath));

            if (links.Count == 0)
            {
                Status = "No images found to upload.";
                OnActionFinished();
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

                    while (WordPressConnector.CanConnectToRW() == false)
                    {
                        for (int second = 20; second > 0; second--)
                        {
                            Status = "No internet connection detected. Trying again in " + (second + 1) +
                                     "seconds. Don't close the window.";
                            await Task.Delay(1000);
                        }
                    }

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
                    
                    OnActionFinished();

                }
            }

            // Update markdown & html
            Console.WriteLine("Starting link replacer...");
            MarkdownText = Converter.ReplaceLocalImageLinksWithUrls(_markdownPath, _htmlPath, OnlyHtml, MarkdownText,
                localImagePaths, imageUrls);
            HtmlText = Converter.ConvertMarkdownStringToHtml(MarkdownText);

            Status = "Upload & replacement complete!";
            OnActionFinished();
        }

        private void OnActionFinished()
        {
            AllowInput = true;
            ProgressValue = ProgressMax;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}