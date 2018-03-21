using Avalonia;
using Avalonia.Markup.Xaml;

namespace MarkdownToRWGUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
