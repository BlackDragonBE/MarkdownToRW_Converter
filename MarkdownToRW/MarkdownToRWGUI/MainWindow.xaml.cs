﻿using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
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