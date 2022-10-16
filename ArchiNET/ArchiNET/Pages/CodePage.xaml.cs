using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodePage : ContentPage
    {
        public CodePage()
        {
            InitializeComponent();
            Title = Resource.CodeTitle;
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Local", (sender) => {
                EnvironmentContext.Current.CodeAnalyser.SetLazyLoading(true).AnalyseAsync(sender.LocalStore.Code, code);
            });
            MessagingCenter.Subscribe<StoreExplorerPage>(this, "Ready_Remote", (sender) => {
                EnvironmentContext.Current.CodeAnalyser.SetLazyLoading(true).AnalyseAsync(sender.RemoteStore.Code, code);
            });
        }
    }
}