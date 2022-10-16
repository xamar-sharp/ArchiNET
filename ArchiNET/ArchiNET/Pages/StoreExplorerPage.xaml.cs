using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArchiNET.Services;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("PreferenceId","id")]
    [QueryProperty("RemoteStoreTitle","title")]
    [QueryProperty("LocalTitle", "localTitle")]
    public partial class StoreExplorerPage : TabbedPage
    {
        public int PreferenceId { get; set; }
        public string RemoteStoreTitle { get; set; }
        public string LocalTitle { get; set; }
        public RemoteStore RemoteStore { get; set; }
        public LocalStore LocalStore { get; set; }
        public StoreExplorerPage()
        {
            InitializeComponent();
            Title = "<--->";
        }
        protected async override void OnAppearing()
        {
            if (RemoteStoreTitle != null)
            {
                RemoteStore = await RestService.GetStore(RemoteStoreTitle);
                MessagingCenter.Send(this, "Ready_Remote");
            }
            else if(LocalTitle == null)
            {
                LocalStore = StoreInitializer.GetStore(PreferenceId);
                MessagingCenter.Send(this, "Ready_Local");
            }
            else
            {
                LocalStore = new LocalStoreProvider().ProvideByTitle(LocalTitle);
                MessagingCenter.Send(this, "Ready_Local");
            }
        }
    }
}