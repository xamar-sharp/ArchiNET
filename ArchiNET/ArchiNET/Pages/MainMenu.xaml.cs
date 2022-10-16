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
    public partial class MainMenu : Shell
    {
        public ImageSource Image { get => ImageSource.FromFile(EnvironmentContext.Current.Profile.IconFileName); set { } }
        public string Login { get => EnvironmentContext.Current.Profile.Login; set { } }
        public string Description { get => EnvironmentContext.Current.Profile.Description; set { } }
        public MainMenu()
        {
            InitializeComponent();
            log.Text = Login;
            desc.Text = Description;
            img.Source = Image;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//profile/0");
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            new ViewModels.UserProfileViewModel(EnvironmentContext.Current.Profile).SignOutCommand.Execute(0);
        }
    }
}