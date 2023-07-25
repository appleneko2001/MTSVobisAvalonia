using MinimalMvvm.Extensions;
using MinimalMvvm.ViewModels;
using MTSVobisAvalonia.Models;

namespace MTSVobisAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public SmsViewerViewModel SmsViewer { get; }

        public MainWindowViewModel()
        {
            SmsViewer = new SmsViewerViewModel();
        }
        
        private int m_SmsInboxCounts;
        public int SmsInboxCounts
        {
            get => m_SmsInboxCounts;
            set => this.SetAndUpdateIfChanged(ref m_SmsInboxCounts, value);
        }

        private int m_SmsInboxMaxCapacity;
        public int SmsInboxMaxCapacity
        {
            get => m_SmsInboxMaxCapacity;
            set => this.SetAndUpdateIfChanged(ref m_SmsInboxMaxCapacity, value);
        }

        private int m_SmsUnreadCounts;
        public int SmsUnreadCounts
        {
            get => m_SmsUnreadCounts;
            set => this.SetAndUpdateIfChanged(ref m_SmsUnreadCounts, value);
        }

        public void UpdateStatus(ModemStatusModel args)
        {
            SmsUnreadCounts = args.SmsUnreadNum; 
        }

        public void UpdateStatus(SmsCapacityInfoModel capInfo)
        {
            SmsInboxCounts = capInfo.SmsReceivedTotal.ParseInt32();
            SmsInboxMaxCapacity = capInfo.SmsCapacityTotal.ParseInt32();
        }
    }
}