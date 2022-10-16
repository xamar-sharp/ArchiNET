using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
using System.Windows.Input;
using System.IO;
using ArchiNET.Constants;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ArchiNET.Services;
using ArchiNET.Pages;
using System.Linq;
namespace ArchiNET.ViewModels
{
    public sealed class LocalStoreViewModel : LiveViewModel
    {
        internal LocalStore _store;
        private readonly ILocalStoreProvider _provider;
        public LocalStoreViewModel(LocalStore store)
        {
            _store = store;
            _provider = new LocalStoreProvider();
            PublishCommand = new Command(async (obj) =>
            {
                HasPublish = await RestService.UploadStore(_store);
                if (HasPublish)
                {
                    _provider.Add(_store);
                }
            },(obj)=>HasPublish);
            IconSelectedCommand = new Command(async (obj) =>
             {
                 await SelectIcon(obj as ImageButton);
             });
            RemoveCommand = new Command(async (obj) =>
            {
                Parent?.Stores?.Remove(this);
                _provider.Remove(_store);
                if (HasPublish)
                {
                    var res = await RestService.DeleteStore(Title);
                }
            });
            SaveChangesCommand = new Command(async (obj) =>
            {
                CreatedAt = DateTime.UtcNow;
                if (HasPublish)
                {
                    PublishCommand?.Execute(0);
                }
                else
                {
                    _provider.Add(_store);
                }
                await Shell.Current.GoToAsync("//store/local/0");
            });
            EditCommand = new Command(async (obj) =>
            {
                await Shell.Current.GoToAsync($"//store/new/0?title={Title}");
            });
        }
        public void OnAllPropertyChanged()
        {
            OnPropertyChanged("Description");
            OnPropertyChanged("Title");
            OnPropertyChanged("Icon");
            OnPropertyChanged("Code");
            OnPropertyChanged("HasPublish");
            OnPropertyChanged("Type");
        }
        public async Task SelectIcon(ImageButton button)
        {
            FileResult res = await MediaPicker.PickPhotoAsync();
            string path;
            using(var stream = await res.OpenReadAsync())
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                path = Path.Combine(EnvironmentContext.ExternalPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(res.FileName));
                File.WriteAllBytes(path, buffer);
                _store.IconFileName = path;
                button.Source = ImageSource.FromFile(path);
            }
        }
        public LocalStoreViewModelList Parent { get; set; }
        public string Title { get => _store.Title; set { _store.Title = value; OnPropertyChanged(); } }
        public string Description { get => _store.Description; set { _store.Description = value; OnPropertyChanged(); } }
        public DateTime CreatedAt { get => _store.CreatedAt; set { _store.CreatedAt = value; OnPropertyChanged(); } }
        public ImageSource Icon { get => ImageSource.FromFile(_store.IconFileName); set { OnPropertyChanged(); } }
        public string Code { get => _store.Code; set { _store.Code = value; OnPropertyChanged(); } }
        public PatternType Type { get => _store.Type; set { _store.Type = value; OnPropertyChanged(); } }
        public bool HasPublish { get => _store.HasPublish; set { _store.HasPublish = value; UpdateState(); OnPropertyChanged(); } }
        public ICommand PublishCommand { get; set; }
        public ICommand IconSelectedCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
    }
}
