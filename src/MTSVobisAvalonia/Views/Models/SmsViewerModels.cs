using Avalonia.Collections;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MTSVobisAvalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;

namespace MTSVobisAvalonia.Views.Models
{
    public class SmsViewerModels : ViewModelBase
    {
        private SmsViewer parent;
        public SmsViewerModels(SmsViewer view)
        {
            m_Messages = new AvaloniaList<SmsDataItemModel>();
            parent = view;
        }

        private AvaloniaList<SmsDataItemModel> m_Messages;
        public AvaloniaList<SmsDataItemModel> Messages => m_Messages;
        private SmsDataItemModel m_SelectedMessage;
        public SmsDataItemModel SelectedMessage
        {
            get => m_SelectedMessage;
            set
            {
                m_SelectedMessage = value;
                this.RaisePropertyChanged();
            }
        }

        //public ReactiveCommand<IList<object>, Unit> DeleteSelectedCommand { get; }

        public async void DeleteSelectedMsgRequest(AvaloniaList<object> selected)
        {
            if (selected is null)
                return;
            if (selected.Count is 0)
                return;
            var list = selected.Cast<SmsDataItemModel>();

            var builder = new StringBuilder(); 
            foreach(var sms in list)
            {
                builder.AppendLine(sms.ToString());
            }

            var dialog = MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams 
                { 
                    ContentTitle = "Confirm action",
                    ContentHeader = $"Are you sure to delete those selected messages ({selected.Count})?",
                    ContentMessage = builder.ToString(),
                    ButtonDefinitions = ButtonEnum.YesNo, 
                });
            if(await dialog.ShowDialog(MainWindow.Instance) == ButtonResult.Yes)
            {
                if (ModemService.Instance.DeleteMessages(list))
                {
                    parent.DeselectAll();
                    ApplySmsBox(ModemService.Instance.GetAllMessages());
                }
                else
                    await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams { ContentTitle = "Action failed", ContentMessage = "Try again later." }).ShowDialog(MainWindow.Instance);
            }
        }

        public async void DeleteAllSms()
        {
            var dialog = MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams()
            {
                ContentTitle = "Confirm action",
                ContentHeader = $"Are you sure to clear all sms?",
                ButtonDefinitions = ButtonEnum.YesNo
            });
            var result = await dialog.ShowDialog(MainWindow.Instance);

            if (result == ButtonResult.Yes)
            {
                ApplySmsBox(ModemService.Instance.GetAllMessages());
            }
        }

        public void RefreshSmsBoxManually()
        {
            var service = ModemService.Instance;
            service.GetAllMessages();
        }

        public void ApplySmsBox (SmsTotalMessagesModel data)
        { 
            Messages.Clear();
            Messages.AddRange(data.Messages);
        }
    }
}
