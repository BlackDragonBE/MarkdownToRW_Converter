using System;
using System.ComponentModel;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DragonMarkdown.Utility;
using MarkdownToRWGUI.Models;
using SkiaSharp;

namespace MarkdownToRWGUI
{
    public class MainWindow : Window
    {
        public TextBox TxtPassword;

        public MainWindow()
        {
            InitializeComponent();
            
            if (File.Exists("rw-logo_250.ico"))
            {
                Bitmap b = new Bitmap("rw-logo_250.ico");
                Icon = new WindowIcon(b);
            }
            
            Closing += OnClosing;
            
            Settings set = SettingsManager.LoadSettings();

            TxtPassword  = this.FindControl<TextBox>("txtPassword");
            
            DataContext = new MainWindowViewModel { ThisWindow = this, TxtPassword = TxtPassword, Status = "Open a markdown file to start!", AllowInput = true, RememberCredentials = set.ShouldLoadCredentials, Username = set.Username, Password = set.Password};
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