using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ArchiNET.Services;
using ArchiNET.Models;
using ArchiNET.Constants;
namespace ArchiNET.ViewModels
{
    public sealed class RemoteStoreViewModel:LiveViewModel
    {
        internal readonly RemoteStore _store;
        public RemoteStoreViewModel(RemoteStore store)
        {
            _store = store;
            RemoveCommand = new Command(async () =>
            {
                Parent.Stores.Remove(this);
                await RestService.DeleteStore(Title);
            });
        }
        public string Title { get => _store.Title; set { _store.Title = value; OnPropertyChanged(); } }
        public string Description { get => _store.Description; set { _store.Description = value; OnPropertyChanged(); } }
        public DateTime CreatedAt { get => _store.CreatedAt; set { _store.CreatedAt = value; OnPropertyChanged(); } }
        public PatternType Type { get => _store.Type; set { _store.Type = value; OnPropertyChanged(); } }
        public ImageSource Icon { get => ImageSource.FromUri(RestService.GetRealURI(_store.IconURI,true)); set { OnPropertyChanged(); } }
        public string Code { get => _store.Code; set { _store.Code = value; OnPropertyChanged(); } }
        public RemoteStoreViewModelList Parent { get; set; }
        public ICommand RemoveCommand { get; set; }
    }
}
