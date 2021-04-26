using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MTSVobisAvalonia.Views.Models;

namespace MTSVobisAvalonia
{
    public class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public ModemService modemService;

        private MainWindowModels Context;

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = Context = new MainWindowModels();
            modemService = ModemService.Instance;
            modemService.ModemStatusUpdated += (s, e) => Context.UpdateStatus(e);
            modemService.MessagesStatusUpdated += (s, e) => Context.UpdateStatus(e);
   
            this.Opened += MainWindow_Opened;

            Instance = this;
        }

        private void MainWindow_Opened(object sender, System.EventArgs e)
        {

        }
         
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
