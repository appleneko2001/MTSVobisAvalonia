using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Avalonia.Collections;
using Avalonia.Controls.Selection;
using MinimalMvvm.Extensions;
using MinimalMvvm.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MTSVobisAvalonia.Models;
using MTSVobisAvalonia.ViewModels.Statusbars;
using MTSVobisAvalonia.Views;

namespace MTSVobisAvalonia.ViewModels
{
    public class SmsViewerViewModel : ViewModelBase
    {
        public ObservableCollection<StatusBarItemViewModel> StatusBarItems => m_StatusBarItems;
        
        public AvaloniaList<SmsDataItemViewModel> Messages => m_Messages;

        public SmsDataItemViewModel? SelectedMessage
        {
            get => m_SelectedMessage;
            private set
            {
                if (this.SetAndUpdateIfChanged(ref m_SelectedMessage, value)) 
                    UpdateSmsReadAsync(value);
            }
        }

        public SelectionModel<SmsDataItemViewModel> Selection => m_Selection;

        private readonly ObservableCollection<StatusBarItemViewModel> m_StatusBarItems;
        private readonly AvaloniaList<SmsDataItemViewModel> m_Messages = new ();
        private SmsDataItemViewModel? m_SelectedMessage;
        private readonly SelectionModel<SmsDataItemViewModel> m_Selection;

        private readonly StatusTextItemViewModel m_SelectedItemsCountText;
        private readonly ModemService m_ModemService;

        public SmsViewerViewModel()
        {
            m_StatusBarItems = new ObservableCollection<StatusBarItemViewModel>
            {
                (m_SelectedItemsCountText = new StatusTextItemViewModel())
            };
            
            m_Selection = new SelectionModel<SmsDataItemViewModel>
            {
                SingleSelect = false
            };
            m_Selection.SelectionChanged += OnSmsSelectionChanged;

            var service = ModemService.Instance;
            service.SmsInboxUpdated += OnModemSmsInboxUpdated;

            m_ModemService = service;
            
            RefreshSmsBoxManually();
        }

        private void OnSmsSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<SmsDataItemViewModel> e)
        {
            var selectedItems = e.SelectedItems;
            SelectedMessage = selectedItems.LastOrDefault();

            UpdateStatusBarText();
        }

        private void UpdateStatusBarText()
        {
            var counts = m_Selection.Count;
            m_SelectedItemsCountText.Text =
                counts == 0 ? "No SMS selected." : $"Selected {counts} SMS.";
        }

        public async void DeleteSelectedMsgRequest(object? param)
        {
            if (param is not SelectionModel<SmsDataItemViewModel> selection)
                return;

            // Get selected items through selection model
            // somehow avaloniaUI works not as I thought lol
            var selected = selection.SelectedItems;
            
            if (selected.Count is 0)
                return;
            
            // Make the selected items as immutable to ready for deletion
            var list = selected
                .Select(a => a?.Model)
                .Where(a => a != null)
                .Cast<SmsDataItemModel>() // Suppress null-possibility warning because we filter out null items
                .ToImmutableArray();

            var builder = new StringBuilder(); 
            foreach(var sms in list)
            {
                builder.AppendLine(sms.ToString());
            }
            
            var dialog = MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams 
                { 
                    ContentTitle = "Confirm action",
                    ContentHeader = $"Are you sure to delete those selected messages ({selected.Count})?",
                    ContentMessage = builder.ToString(),
                    ButtonDefinitions = ButtonEnum.YesNo, 
                });

            // If the user changed their mind and not about to delete, then just abort this procedure. 
            if (await dialog.ShowWindowDialogAsync(MainWindow.Instance) != ButtonResult.Yes)
                return;
            
            // otherwise, complete the deletion procedure.
            if (await ModemService.Instance.DeleteMessagesAsync(list))
            {
                Selection.Clear();
                ApplySmsBox(ModemService.Instance.GetAllReceivedMessages());
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams
                    {
                        ContentTitle = "Action failed", 
                        ContentMessage = "Try again later."
                    }).ShowWindowDialogAsync(MainWindow.Instance);
            }
        }
        
        public async void DeleteAllSms()
        {
            var dialog = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams()
            {
                ContentTitle = "Confirm action",
                ContentHeader = $"Are you sure to clear all sms?",
                ButtonDefinitions = ButtonEnum.YesNo
            });
            var result = await dialog.ShowWindowDialogAsync(MainWindow.Instance);

            if (result != ButtonResult.Yes)
                return;
            
            var msgs = Messages
                .ToImmutableArray()
                .Select(a => a.Model)
                .ToImmutableArray();
                
            // Since in vobisJS source those chinese pinned the "Delete all SMS" features are removed already
            // we gonna select all possible received SMS and delete them directly.
            // would make the HTTP request way too long if your modem receives too much SMS more than 100
            // (if you have modded your modem firmware and storage unit ig)  
            // Probably the entry is still usable, please tell me via creating an github issue
            /*
                 * 删除全部短消息
                 * -- 目前经UE确认，移除了删除全部短信功能。此方法暂时保留
                 * 
                 * @method deleteAllMessages
                 * From /js/config/service.js
                 */ 
            if (await ModemService.Instance.DeleteMessagesAsync(msgs))
            {
                Selection.Clear();
                ApplySmsBox(ModemService.Instance.GetAllReceivedMessages());
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams
                    {
                        ContentTitle = "Action failed", 
                        ContentMessage = "Try again later."
                    }).ShowWindowDialogAsync(MainWindow.Instance);
            }
        }

        public async void UpdateSmsReadAsync(SmsDataItemViewModel? viewModel)
        {
            if(viewModel == null)
                return;
            
            if(!viewModel.IsUnread)
                return;
            
            viewModel.SetRead();
            await m_ModemService.SetSmsReadAsync(new []
            {
                viewModel.Id
            });
        }
        
        public async void RefreshSmsBoxManually()
        {
            var service = ModemService.Instance;
            var result = await service.GetAllReceivedMessagesAsync();
            ApplySmsBox(result);
        }
        
        public void ApplySmsBox (SmsTotalMessagesModel? data)
        {
            Messages.Clear();

            var msgs = data?.Messages;

            if (msgs == null)
                return;
            
            Messages.AddRange(msgs
                .Select(a=> new SmsDataItemViewModel(a)));
        }

        private void OnModemSmsInboxUpdated(object? sender, SmsTotalMessagesModel e)
        {
            
        }
    }
}