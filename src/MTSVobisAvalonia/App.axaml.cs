using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace MTSVobisAvalonia
{
    public class App : Application
    {
        public ModemService modemService;

        public override void Initialize()
        {
            try
            {
                var ip = "ip_address_target".ReadAllText();
                modemService = new ModemService(ip);

                AvaloniaXamlLoader.Load(this); 
            }
            catch(Exception e)
            {
                
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
