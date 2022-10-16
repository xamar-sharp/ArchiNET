using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.ViewModels;
using ArchiNET.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyStoresPage : ContentPage
    {
        public LocalStoreViewModelList ViewModel { get; set; }
        public MyStoresPage()
        {
            InitializeComponent();
            Title = Resource.MyStoresTitle;
            BindingContext = ViewModel = new LocalStoreViewModelList(new ArchiNET.Services.LocalStoreProvider().Provide().ToList());
        }
        protected override void OnAppearing()
        {
            ViewModel.Stores = new System.Collections.ObjectModel.ObservableCollection<LocalStoreViewModel>(new ArchiNET.Services.LocalStoreProvider().Provide().Select(ent => new LocalStoreViewModel(ent) {Parent=ViewModel}));
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.ReadCommand.Execute((sender as ListView).SelectedItem);
        }
    }
}