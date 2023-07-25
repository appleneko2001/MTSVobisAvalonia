using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MTSVobisAvalonia.ViewModels;

namespace MTSVobisAvalonia.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance
        {
            get;
            private set;
        }
        
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            
            var viewModel = new MainWindowViewModel();
            var modemService = ModemService.Instance;
            modemService.ModemStatusUpdated += (s, e) => viewModel.UpdateStatus(e);
            modemService.MessagesStatusUpdated += (s, e) => viewModel.UpdateStatus(e);

            DataContext = viewModel;
            
            this.AttachDevTools();
        }
         
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
