using MinimalMvvm.Extensions;

namespace MTSVobisAvalonia.ViewModels.Statusbars
{
    public class StatusTextItemViewModel : StatusBarItemViewModel
    {
        public string? Text
        {
            get => m_Text;
            set => this.SetAndUpdateIfChanged(ref m_Text, value);
        }
        private string? m_Text;
    }
}