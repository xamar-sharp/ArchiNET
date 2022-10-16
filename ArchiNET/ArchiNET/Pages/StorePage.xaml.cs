using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArchiNET.Models;
using ArchiNET.ViewModels;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StorePage : ContentPage
    {
        public LocalStoreViewModel LocalStore { get; set; }
        public RemoteStoreViewModel RemoteStore { get; set; }
        public StorePage()
        {
            Title = Resource.StoreTitle;
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Remote", (sender) =>
            {
                this.BindingContext = RemoteStore = new RemoteStoreViewModel(sender.RemoteStore);
            });
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Local", (sender) => {
                this.BindingContext = LocalStore = new LocalStoreViewModel(sender.LocalStore);
            });
            InitializeComponent();
        }
    }
}