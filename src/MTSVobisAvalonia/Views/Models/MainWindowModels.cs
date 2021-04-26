using MTSVobisAvalonia.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTSVobisAvalonia.Views.Models
{
    public class MainWindowModels : ViewModelBase
    {
        private int m_SmsInboxCounts;
        public int SmsInboxCounts
        {
            get => m_SmsInboxCounts;
            set { m_SmsInboxCounts = value; this.RaisePropertyChanged(); }
        }

        private int m_SmsInboxMaxCapacity;
        public int SmsInboxMaxCapacity
        {
            get => m_SmsInboxMaxCapacity;
            set { m_SmsInboxMaxCapacity = value; this.RaisePropertyChanged(); }
        }

        private int m_SmsUnreadCounts;
        public int SmsUnreadCounts
        {
            get => m_SmsUnreadCounts;
            set { m_SmsUnreadCounts = value; this.RaisePropertyChanged(); }
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
