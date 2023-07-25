using System;
using MinimalMvvm.Extensions;
using MinimalMvvm.ViewModels;
using MTSVobisAvalonia.Models;

namespace MTSVobisAvalonia.ViewModels
{
    public class SmsDataItemViewModel : ViewModelBase
    {
        private readonly SmsDataItemModel m_Model;
    
        public SmsDataItemViewModel(SmsDataItemModel model)
        {
            m_Model = model;

            IsUnread = m_Model.TagString == "1";
        }

        public string Id => m_Model.Id;
        public string From => m_Model.From;
        public string Content =>m_Model.Content;
        public DateTime When => m_When ??= m_Model.DateString.TransTime();
        public string TagString => m_Model.TagString;

        private DateTime? m_When;
        private bool m_IsUnread;
    
        public string ContentPreview => Content.Trimming(30);

        public string GetClasses => TagString == "1" ? "Unread" : "";

        public bool IsUnread
        {
            get => m_IsUnread;
            private set => this.SetAndUpdateIfChanged(ref m_IsUnread, value);
        }

        public SmsDataItemModel Model => m_Model;

        public void SetRead()
        {
            m_Model.TagString = "0";
            IsUnread = false;
        }
    
        public override string ToString()
        {
            return $"{From}: {ContentPreview}";
        }
    }
}