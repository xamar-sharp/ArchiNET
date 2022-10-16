using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using ArchiNET.Models;
using ArchiNET.Services;
namespace ArchiNET.ViewModels
{
    public sealed class LocalStoreViewModelList:LiveViewModel
    {
        private ObservableCollection<LocalStoreViewModel> _stores;
        public ObservableCollection<LocalStoreViewModel> Stores { get => _stores; set { _stores = value;OnPropertyChanged(); } }
        public ICommand GetStoresCommand { get; set; }
        public ICommand ReadCommand { get; set; }
        public LocalStoreViewModelList(IList<LocalStore> stores)
        {
            Stores = new ObservableCollection<LocalStoreViewModel>(stores.Select(ent => new LocalStoreViewModel(ent) {Parent=this}));
            GetStoresCommand = new Command((obj) =>
            {
                Stores = new ObservableCollection<LocalStoreViewModel>(new LocalStoreProvider().Provide().Select(ent => new LocalStoreViewModel(ent) {Parent=this}));
            });
            ReadCommand = new Command(async (obj) =>
            {
                var store = obj as LocalStoreViewModel;
                if (store.HasPublish)
                {
                    await Shell.Current.GoToAsync($"//patterns/{store.Type.ToString().ToLowerInvariant()}/0?title={store.Title}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"//patterns/{store.Type.ToString().ToLowerInvariant()}/0?localTitle={store.Title}");
                }
            });
        }
    }
}
