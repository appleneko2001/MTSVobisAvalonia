using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace MTSVobisAvalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            var ip = "ip_address_target".ReadAllText();
            ModemService.Init(ip);
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Views.MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
