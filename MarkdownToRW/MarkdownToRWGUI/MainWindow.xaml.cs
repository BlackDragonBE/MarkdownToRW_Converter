using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarkdownToRWGUI.Models;

namespace MarkdownToRWGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel {Greeting = "Enter a name and click the button.", ThisWindow = this, Status = "Ready to convert!"};
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoaderPortableXaml.Load(this);
        }

        public void OnButtonClicked(object sender, RoutedEventArgs args)
        {
            var context = DataContext as MainWindowViewModel;
            context.Status = "Hi " + context.Name + " !";
        }
    }
}