using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using ChatView.Models;
using ChatView;
using System.Linq;
using ArchiNET.Services;
using ArchiNET.Pages;
namespace ArchiNET.ViewModels
{
   public sealed  class MessageViewModelList:LiveViewModel
    {
        private ObservableCollection<ChatView.Models.Message> _messages;
        public ObservableCollection<ChatView.Models.Message> Messages { get => _messages; set { _messages = value;OnPropertyChanged(); } }
        public ICommand GetMessagesCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand OpenProfileCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public MessageViewModelList(IList<ArchiNET.Models.Message> messages)
        {
            Messages = new ObservableCollection<Message>(messages.Parse());
            GetMessagesCommand = new Command(async (obj) =>
            {
                ChatView.ChatView tuple = obj as ChatView.ChatView;
                var val = (await RestService.GetMessages());
                if (val.Count > 0)
                {
                    Messages = new ObservableCollection<Message>(val.Parse());
                    tuple.Messages = Messages;
                }
            });
            SendCommand = new Command(async (obj) =>
            {
                await RestService.SendMessage(obj as string);
            });
            OpenProfileCommand = new Command(async (obj) =>
            {
                await Shell.Current.GoToAsync("//profile?login="+((Message)obj).SenderLogin);
            });
            RemoveCommand = new Command(async (obj) =>
            {
                var selected = (Message)(obj as ListView).SelectedItem;
                if (selected.SenderLogin == EnvironmentContext.Current.UserProvider.Provide().Login)
                {
                    Messages.Remove(selected);
                    await RestService.DeleteMessage(selected.CreatedAt, selected.TextMessage);
                }
            });
        }
    }
    public static class MessageListExtensions
    {
        public static IEnumerable<ChatView.Models.Message> Parse(this IList<Models.Message> messages)
        {
            return messages.Select(ent => new MessageDTO() { CreatedAt = ent.CreatedAt, Login = ent.UserName, IconURI = RestService.GetRealURI(ent.UserIconUri,false).AbsolutePath, TextMessage = ent.Text }).Select(ent => new Message(ent));
        }
    }
}
