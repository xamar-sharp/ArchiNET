using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.ViewModels;
using Xamarin.Forms;
using ArchiNET.Services;
using Xamarin.Forms.Xaml;

namespace ArchiNET.Pages
{
    [QueryProperty("UserLogin","login")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public string UserLogin { get; set; }
        public UserProfileViewModel ViewModel { get; set; }
        public ProfilePage()
        {
            InitializeComponent();
            Title = Resource.ProfileTitle;
            ViewModel = new UserProfileViewModel(EnvironmentContext.Current.UserProvider.Provide());
            this.BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            if(UserLogin != null)
            {
                ViewModel = new UserProfileViewModel(await RestService.SearchUser(UserLogin));
            }
        }
    }
}