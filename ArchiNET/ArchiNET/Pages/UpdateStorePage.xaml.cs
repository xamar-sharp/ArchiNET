using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.Models;
using ArchiNET.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("LocalTitle", "title")]
    public partial class UpdateStorePage : ContentPage
    {
        private LocalStoreViewModel _viewModel;
        public LocalStoreViewModel ViewModel { get => _viewModel; set { _viewModel = value;OnPropertyChanged(); } }
        public IList<string> PatternTypes { get; set; }
        public string LocalTitle { get; set; }
        public UpdateStorePage()
        {
            InitializeComponent();
            Title = Resource.UploadTitle;
            PatternTypes = Enum.GetNames(typeof(Constants.PatternType)).ToList();
            ViewModel = new LocalStoreViewModel(new LocalStore());
            this.BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(LocalTitle))
            {
                var val = new ArchiNET.Services.LocalStoreProvider().ProvideByTitle(LocalTitle);
                _viewModel._store = val;
                ViewModel.OnAllPropertyChanged();
            }
        }
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.Type = (Constants.PatternType)Enum.Parse(typeof(Constants.PatternType), (sender as Picker).SelectedItem as string);
        }
    }
}