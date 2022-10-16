using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ArchiNET.Services;
using ArchiNET.Models;
using System.Linq;
namespace ArchiNET.ViewModels
{
    public sealed class RemoteStoreViewModelList:LiveViewModel
    {
        private ObservableCollection<RemoteStoreViewModel> _stores;
        public ObservableCollection<RemoteStoreViewModel> Stores { get => _stores; set { _stores = value; OnPropertyChanged(); } }
        public ICommand GetStoreCommand { get; set; }
        public ICommand GetStoresCommand { get; set; }
        public ICommand ReadCommand { get; set; }
        public int Offset { get; set; }
        public RemoteStoreViewModelList(IList<RemoteStore> stores)
        {
            Stores = new ObservableCollection<RemoteStoreViewModel>(stores.Select(ent=>new RemoteStoreViewModel(ent)));
            GetStoreCommand = new Command(async (obj) =>
            {
                var val = await RestService.GetStore(obj as string);
                if (val != null)
                {
                    Stores.Clear();
                    Stores.Add(new RemoteStoreViewModel(val));
                }
            });
            GetStoresCommand = new Command(async (obj) =>
            {
                (obj as RefreshView).IsRefreshing = true;
                var remote = await RestService.GetStores(EnvironmentContext.Current.TopRemote, Offset);
                Offset += EnvironmentContext.Current.TopRemote;
                Stores = new ObservableCollection<RemoteStoreViewModel>(remote.Select(ent => new RemoteStoreViewModel(ent)));
                (obj as RefreshView).IsRefreshing = false;
            });
            ReadCommand = new Command(async (obj) =>
            {
                var model = obj as RemoteStoreViewModel;
                await Shell.Current.GoToAsync($"//patterns/{model.Type.ToString().ToLowerInvariant()}/0?title={model.Title}");
            });
        }
    }
}
