using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using System.IO;
using ArchiNET.Services;
using ArchiNET.Pages;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ArchiNET.Models;
namespace ArchiNET.ViewModels
{
    public sealed class UserProfileViewModel : LiveViewModel
    {
        internal readonly UserProfile _profile;
        public Page Parent { get; set; }
        public string Login { get => _profile.Login; set { _profile.Login = value; OnPropertyChanged(); } }
        public string Description { get => _profile.Description; set { _profile.Description = value; OnPropertyChanged(); } }
        public ImageSource Icon { get => ImageSource.FromFile(_profile.IconFileName); set { _profile.IconFileName = (value as FileImageSource).File; OnPropertyChanged(); } }
        public DateTime CreatedAt { get => _profile.CreatedAt; set { _profile.CreatedAt = value; OnPropertyChanged(); } }
        public ICommand SignOutCommand { get; set; }
        public ICommand SignUpCommand { get; set; }
        public ICommand IconSelectedCommand { get; set; }
        public UserProfileViewModel(UserProfile profile)
        {
            _profile = profile;
            SignUpCommand = new Command(async () =>
            {
                if (await RestService.AuthorizeAsync(_profile))
                {
                    App.Current.MainPage = new MainMenu();
                }
                else
                {
                    await Parent.DisplayAlert(Resource.Message, Resource.AuthorizationError, Resource.Cancel);
                }
            });
            SignOutCommand = new Command(async () =>
            {
                App.Current.MainPage = new Pages.RegistrationPage();
                await RestService.SignOutAsync();
            });
            IconSelectedCommand = new Command(async (obj) => { 

                (obj as ImageButton).Source = ImageSource.FromFile(await SelectIcon());
            });
        }
        public async Task<string> SelectIcon()
        {
            FileResult res = await MediaPicker.PickPhotoAsync();
            string path;
            using (var stream = await res.OpenReadAsync())
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                path = Path.Combine(EnvironmentContext.ExternalPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(res.FileName));
                File.WriteAllBytes(path, buffer);
                _profile.IconFileName = path;
                return _profile.IconFileName;
            }
        }
    }
}
