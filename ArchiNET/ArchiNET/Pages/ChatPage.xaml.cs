using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.ViewModels;
using Xamarin.Forms;
using ArchiNET.Services;
using Xamarin.Forms.Xaml;

namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public MessageViewModelList ViewModel { get; set; }
        public ChatPage()
        {
            InitializeComponent();
            Title = Resource.ChatPageTitle;
            ViewModel = new MessageViewModelList(new List<Models.Message>());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupChat();
        }
        public void SetupChat()
        {
            var user = EnvironmentContext.Current.UserProvider.Provide();
            chat.Messages = new System.Collections.ObjectModel.ObservableCollection<ChatView.Models.Message>();
            chat.UserName = user.Login;
            chat.ContextCommand = ViewModel.RemoveCommand;
            chat.IconTappedCommand = ViewModel.OpenProfileCommand;
            chat.RefreshCommand = ViewModel.GetMessagesCommand;
            chat.ContextIcon = "remove.png";
            chat.UserIcon = user.IconFileName;
            chat.MessageSended += (obj,args) =>
            {
                ViewModel.SendCommand.Execute(chat.TextMessage);
                ViewModel.GetMessagesCommand.Execute(chat);
            };
            var converter = EnvironmentContext.Current.ColorConverter;
            chat.BackColor = converter.ToColor(EnvironmentContext.Current.TextBarBackColor);
            chat.TextColor = converter.ToColor(EnvironmentContext.Current.TextBarColor);
            chat.RefreshColor = converter.ToColor(EnvironmentContext.Current.RefreshColor);
            chat.BackgroundImage = EnvironmentContext.Current.ChatImage;
            chat.SendImage = EnvironmentContext.Current.ChatSendImage;
            chat.ContentDateColor = converter.ToColor(EnvironmentContext.Current.ChatDateColor);
            chat.ContentTitleColor = converter.ToColor(EnvironmentContext.Current.ChatAvatarColor);
            chat.Pair = new KeyValuePair<Color, Color>(EnvironmentContext.Current.ColorConverter.ToColor(Resource.CornflowerBlue), chat.BackColor);
            chat.ContentTextColor = converter.ToColor(EnvironmentContext.Current.ChatMessageColor);
        }
    }
}