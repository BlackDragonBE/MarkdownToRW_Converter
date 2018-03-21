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
        private string _greeting;
        private bool _isClassy;
        private bool _uploadImages;
        private string _name;
        private string _markdownText;
        private string _htmlText;
        private string _result;

        private ICommand _startConvertCommand;
        private ICommand _testCommand;

        public Window ThisWindow;
        
        public ICommand StartConvertCommand =>
            _startConvertCommand ?? (_startConvertCommand = new CommandHandler(Convert, true));

        public ICommand TestCommand => _testCommand ?? (_testCommand = new CommandHandler(DoTest, true));

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

        public string Greeting
        {
            get => _greeting;
            set
            {
                if (value != _greeting)
                {
                    _greeting = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                if (value != _result)
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsClassy
        {
            get => _isClassy;
            set
            {
                if (value != _isClassy)
                {
                    _isClassy = value;
                    OnPropertyChanged();
                    Name = "Classy as fuck: " + _isClassy;
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
                    }
                }

            }
            else
            {
                Result = "No valid markdown chosen!";
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

        public void DoTest()
        {
            Result = "Test succesful";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}