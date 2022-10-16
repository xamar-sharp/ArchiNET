using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ArchiNET.Services;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public RemoteStoreViewModelList ViewModel { get; set; }
        public SearchPage()
        {
            InitializeComponent();
            Title = Resource.SearchTitle;
            ViewModel = new RemoteStoreViewModelList(new List<Models.RemoteStore>());
            this.BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            var values = (await RestService.GetStores(EnvironmentContext.Current.TopRemote, 0));
            if (values != null)
            {
                ViewModel.Stores = new ObservableCollection<RemoteStoreViewModel>(values.Select(ent => new RemoteStoreViewModel(ent)));
            }
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.ReadCommand.Execute((e.Item as RemoteStoreViewModel));
        }
    }
}