using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DragonMarkdown;
using DragonMarkdown.Updater;
using DragonMarkdown.Utility;
using MarkdownToRWGUI.Models;

namespace MarkdownToRWGUI
{
    public class MainWindow : Window
    {
        public Button BtnDownloadUpdate;
        public TextBox TxtPassword;

        public MainWindow()
        {
            InitializeComponent();
                       
            Title += " v" + DragonVersion.VERSION;

            if (File.Exists("rw-logo_250.ico"))
            {
                Bitmap b = new Bitmap("rw-logo_250.ico");
                Icon = new WindowIcon(b);
            }

            Closing += OnClosing;

            Settings set = SettingsManager.LoadSettings();

            TxtPassword = this.FindControl<TextBox>("txtPassword");
            BtnDownloadUpdate = this.FindControl<Button>("btnDownloadUpdate");


            DataContext = new MainWindowViewModel
            {
                ThisWindow = this,
                TxtPassword = TxtPassword,
                Status = "Open a markdown file to start!",
                AllowInput = true,
                RememberCredentials = set.ShouldLoadCredentials,
                Username = set.Username,
                Password = set.Password,
                FirstImageRight = true,
                Settings = set,
                SaveConverterSettings = set.RememberConverterSettings
            };

            if (set.RememberConverterSettings)
            {
                ((MainWindowViewModel) DataContext).SaveOutputToHtml = set.OutputToHtml;
                ((MainWindowViewModel) DataContext).FirstImageRight = set.ConverterOptions.FirstImageIsAlignedRight;
                ((MainWindowViewModel) DataContext).ReplaceImageAlts = set.ConverterOptions.ReplaceImageWithAltWithCaption;
                ((MainWindowViewModel) DataContext).UseContentScanner = set.UseContentScanner;
            }

            CheckForUpdate();
        }

        private async void CheckForUpdate()
        {
            await Task.Delay(25);
            ((MainWindowViewModel) DataContext).Status = "Checking for updates...";
            await Task.Delay(25);
            GithubRelease newRealease = UpdateHelper.CheckForUpdates();

            if (newRealease == null)
            {
                ((MainWindowViewModel) DataContext).Status = "Application is up to date.";
            }
            else
            {
                ((MainWindowViewModel) DataContext).Status =
                    "New version available: " + newRealease.name + ". Check the console for more information.";
                ((MainWindowViewModel) DataContext).NewUpdate = true;
                ((MainWindowViewModel) DataContext).UpdateDownloadUrl = newRealease.html_url;
                BtnDownloadUpdate.Content = "Download update: " + newRealease.name;
                ((MainWindowViewModel)DataContext).NewRelease = newRealease;

            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoaderPortableXaml.Load(this);
        }
    }
}