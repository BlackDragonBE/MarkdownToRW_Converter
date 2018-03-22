using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using DragonMarkdown;
using DragonMarkdown.Utility;

namespace MarkdownToRWGUI.Models
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // UI
        private bool _onlyHtml;
        private bool _saveOutputToHtml;
        private string _markdownText;
        private string _htmlText;
        private string _status;
        private string _username;
        private string _password;

        private bool _allowInput = true;
        private bool _markdownLoaded = false;

        private ICommand _startConvertCommand;
        private ICommand _showPreviewCommand;
        private ICommand _uploadImagesCommand;

        // Logic
        private string _markdownPath = null;
        private string _htmlPath = null;
        private string _htmlPreviewPath = null;

        public Window ThisWindow;

        public ICommand StartConvertCommand =>
            _startConvertCommand ?? (_startConvertCommand = new CommandHandler(Convert, AllowInput));

        public ICommand ShowPreviewCommand =>
            _showPreviewCommand ?? (_showPreviewCommand = new CommandHandler(ShowPreview, AllowInput));

        public ICommand UploadImagesCommand =>
            _uploadImagesCommand ?? (_uploadImagesCommand = new CommandHandler(UploadImages, AllowInput));

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
                    OnPropertyChanged();
                }
            }
        }

        private async void Convert()
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
                        MarkdownText = sr.ReadToEnd().Replace("\t","  ");
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
            markdownFilter.Extensions.Add("rmd"); markdownFilter.Name = "Markdown Files";

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

        private void ShowPreview()
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

        public void UploadImages()
        {
            Status = "Testing connection...";
            WordPressConnector.InitializeWordPress(Username, Password);

            if (WordPressConnector.CanConnectToRW())
            {
                Status = "Initial connection to RW WordPress OK.";
            }
            else
            {
                Status = "Connection failed. Can't connect to RW.";
            }
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}