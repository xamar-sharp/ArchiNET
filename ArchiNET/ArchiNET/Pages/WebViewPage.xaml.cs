using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage()
        {
            InitializeComponent();
            Title = Resource.WebViewTitle;
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Local", (sender) => representation.Source = new UrlWebViewSource()
            {
                Url =
                  "https://yandex.ru/search?clid=210&text=" + sender.LocalStore.Title + " Pattern"
            });
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Remote", (sender) => representation.Source = new UrlWebViewSource()
            {
                Url =
                  "https://yandex.ru/search?clid=210&text=" + sender.RemoteStore.Title + " Pattern"
            });
        }
    }
}