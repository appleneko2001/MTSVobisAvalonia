using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MTSVobisAvalonia.Models;
using MTSVobisAvalonia.Views.Models;
using System.Collections.ObjectModel;

namespace MTSVobisAvalonia.Views
{
    public class SmsViewer : UserControl
    {
        public ModemService modemService;

        public ListBox SmsListBox;

        public SmsViewerModels Context;

        public SmsViewer()
        {
            modemService = ModemService.Instance;
            modemService.SmsInboxUpdated += (s, e) => Context.ApplySmsBox(e);
            Context = new SmsViewerModels(this);
            this.InitializeComponent();

            DataContext = Context;

            SmsListBox = this.Get<ListBox>("SmsListBox");
            SmsListBox.SelectionChanged += SmsListBox_SelectionChanged;

            this.AttachedToVisualTree += SmsViewer_AttachedToVisualTree;
        }

        public void DeselectAll() => SmsListBox.SelectedIndex = -1;

        private void SmsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItems.Count == 1)
            {
                
                var sms = listBox.SelectedItem as SmsDataItemModel;
                Context.SelectedMessage = sms;
                SetReadedIfUnread(sms);
            }
        }

        private void SetReadedIfUnread(SmsDataItemModel sms)
        {
            if (sms.IsUnread)
            {
                sms.SetRead(modemService.SetSmsReaded(sms.Id));
            }
        }

        private void SmsViewer_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            ReloadSmsInbox();
        }

        private void ReloadSmsInbox()
        {
            var result = modemService.GetAllMessages();
            Context.ApplySmsBox(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
