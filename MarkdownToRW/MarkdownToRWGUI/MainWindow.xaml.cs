using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using MarkdownToRWGUI.Models;

namespace MarkdownToRWGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("rw-logo_250.ico"))
            {
                Bitmap b = new Bitmap("rw-logo_250.ico");
                Icon = new WindowIcon(b);
            }

            Settings set = SettingsManager.LoadSettings();

            TextBox txtPassword = this.FindControl<TextBox>("txtPassword");

            DataContext = new MainWindowViewModel { ThisWindow = this, TxtPassword = txtPassword, Status = "Open a markdown file to start!", AllowInput = true, RememberCredentials = set.ShouldLoadCredentials, Username = set.Username, Password = set.Password};
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoaderPortableXaml.Load(this);
        }
    }
}