using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;

namespace MarkdownToRWGUI.Models
{
    internal class HelloViewModel : INotifyPropertyChanged
    {
        private string _greeting;
        private bool _isClassy;
        private string _name;
        private string _result;

        private ICommand _startConvertCommand;
        private ICommand _testCommand;

        public ICommand StartConvertCommand =>
            _startConvertCommand ?? (_startConvertCommand = new CommandHandler(Convert, true));

        public ICommand TestCommand => _testCommand ?? (_testCommand = new CommandHandler(DoTest, true));

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

        public void Convert()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.AllowMultiple = false;

            FileDialogFilter markdownFilter = new FileDialogFilter();
            markdownFilter.Extensions.Add("md");
            markdownFilter.Extensions.Add("markdown");
            markdownFilter.Extensions.Add("txt");
            markdownFilter.Extensions.Add("mdtxt");
            markdownFilter.Name = "Markdown Files";

            FileDialogFilter allFilter = new FileDialogFilter();
            allFilter.Extensions.Add("*");
            allFilter.Name = "All Files";

            openDialog.Filters.Add(markdownFilter);
            openDialog.Filters.Add(allFilter);
            Task<string[]> resultString = openDialog.ShowAsync();

            if (resultString.Result != null && resultString.Result.Length > 0)
            {
                Result = "Converting " + resultString.Result[0];
            }
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