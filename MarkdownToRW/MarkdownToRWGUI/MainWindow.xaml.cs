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
            Icon = new WindowIcon("");
            DataContext = new MainWindowViewModel { ThisWindow = this, Status = "Ready to convert!"};
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoaderPortableXaml.Load(this);
        }
    }
}