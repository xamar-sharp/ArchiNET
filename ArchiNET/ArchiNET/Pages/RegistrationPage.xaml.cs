using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArchiNET.Services;
using ArchiNET.ViewModels;

namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public UserProfileViewModel Model { get; set; }
        public RegistrationPage()
        {
            InitializeComponent();
            Title = Resource.RegistrationTitle;
            this.BindingContext = Model = new UserProfileViewModel(new Models.UserProfile());
        }
    }
}