using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChatView.Converters;
using ArchiNET.Services;
using ArchiNET.Pages;
namespace ArchiNET
{
    public partial class App : Application
    {
        public App()
        {
            StoreInitializer.Init();
            EnvironmentContext.Current.SetupContext();
            InitializeComponent();
        }
        protected override async void OnStart()
        {
            if (!RestService._userProvider.CanProvide)
            {
                MainPage = new RegistrationPage();
                if(await RestService.AuthorizeAsync())
                {
                    MainPage = new MainMenu();
                }
            }
            else
            {
                MainPage = new MainMenu();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
