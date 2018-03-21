using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using DragonMarkdown;

namespace MarkdownToRWGUI.Models
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _onlyHtml;
        private bool _uploadImages;
        private bool _saveOutputToHtml;
        private string _markdownText;
        private string _htmlText;
        private string _status;

        private bool _allowInput = true;
        private bool _markdownLoaded = false;

        private ICommand _startConvertCommand;

        public Window ThisWindow;
        
        public ICommand StartConvertCommand =>
            _startConvertCommand ?? (_startConvertCommand = new CommandHandler(Convert, AllowInput));

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

        public bool UploadImages
        {
            get => _uploadImages;
            set
            {
                if (value != _uploadImages)
                {
                    _uploadImages = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public async void Convert()
        {
            string path = await ChooseFile();

            if (path != null)
            {

                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        MarkdownText = sr.ReadToEnd().Replace("\t","  ");
                        HtmlText = Converter.ConvertMarkdownStringToHtml(MarkdownText);
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

        public async Task<string> ChooseFile()
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}