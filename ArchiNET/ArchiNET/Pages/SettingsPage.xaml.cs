using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.Forms.Xaml;
using ArchiNET.Constants;
using ArchiNET.Services;
namespace ArchiNET.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public ObservableCollection<Preset> Presets { get; set; }
        public IList<ConnectionType> ConnectionConstraints { get; set; }
        public IList<string> DefaultColors { get; set; }
        public IList<string> SupportedCultures { get; set; }
        public IList<string> AvailableColors { get; set; }
        private IDictionary<string, string> CultureMap { get; }
        private IDictionary<string, Color> ColorMap { get; }
        public SettingsPage()
        {
            InitializeComponent();
            Title = Resource.SettingsTitle;
            CultureMap = new Dictionary<string, string>(2) { [Resource.Russian] = "ru-RU", [Resource.English] = "en-US" };
            SupportedCultures = CultureMap.Keys.ToList();
            ColorMap = new Dictionary<string, Color>(4) { { Resource.White, Color.White }, { Resource.Black, Color.Black }, { Resource.CornflowerBlue, Color.CornflowerBlue }, { Resource.Green, Color.Green } };
            ConnectionConstraints = EnvironmentContext.Current.ConnectionMap.Keys.ToList();
            Presets = new ObservableCollection<Preset>(EnvironmentContext.Current.PresetMap.Select(el => new Preset() { Key = el.Key, Color = el.Value, Parent = this }).ToList());
            DefaultColors = new List<string>(4) { Resource.White, Resource.Black, Resource.CornflowerBlue, Resource.Green };
            this.BindingContext = this;
        }
        public async Task<string> SelectImage(ImageButton button)
        {
            FileResult res = await MediaPicker.PickPhotoAsync();
            button.Source = ImageSource.FromFile(res.FullPath);
            return res.FullPath;
        }
        public async Task SetupDefaultValues()
        {
            if (EnvironmentContext.Current.HasBackup)
            {
               await EnvironmentContext.Current.LoadAsync();
            }
            topRemote.Text = EnvironmentContext.Current.TopRemote.ToString();
            culture.SelectedItem = CultureMap.FirstOrDefault(ent => ent.Value == EnvironmentContext.Current.DefaultCulture).Key;
            connLimit.Text = EnvironmentContext.Current.ConnectionLimit.ToString();
            dnsTimeout.Text = EnvironmentContext.Current.DnsTimeout.ToString();
            connConstraint.SelectedItem = EnvironmentContext.Current.ConnectionConstraint;
            defaultColor.SelectedItem = EnvironmentContext.Current.DefaultTextColor;
            lastMessages.Value = EnvironmentContext.Current.LastMessagesCount;
            useAnim.IsChecked = EnvironmentContext.Current.UseAnimations;
            isDark.IsToggled = EnvironmentContext.Current.IsDarkTheme;
        }
        protected override async void OnAppearing()
        {
            await SetupDefaultValues();
        }
        public sealed class Preset
        {
            public string Key { get; set; }
            public Color Color { get; set; }
            public Command RemoveCommand { get; set; }
            public Command PostCommand { get; set; }
            public SettingsPage Parent { get; internal set; }
            public Preset()
            {
                RemoveCommand = new Command(() =>
                {
                    if (EnvironmentContext.Current.PresetMap.Remove(Key))
                    {
                        Parent.Presets.Remove(this);
                    }
                });
                PostCommand = new Command(async () =>
                {
                    var map = await Parent.DisplayPromptAsync(Resource.Message, Resource.EnterPresetFormat, placeholder: Resource.EnterPresetFormat, cancel: Resource.Cancel, keyboard: Keyboard.Text, maxLength: 100);
                    string[] values = map.Split('=');
                    if (values.Length != 2)
                    {
                        await Parent.DisplayAlert(Resource.Message, Resource.InvalidPresetFormat, Resource.Cancel);
                    }
                    else
                    {
                        switch (values[1])
                        {
                            case "Red":
                                EnvironmentContext.Current.PresetMap.Add(values[0], Color.Red);
                                Parent.Presets.Add(new Preset() { Parent = Parent, Key = values[0], Color = Color.Red });
                                break;
                            case "Green":
                                EnvironmentContext.Current.PresetMap.Add(values[0], Color.Green);
                                Parent.Presets.Add(new Preset() { Parent = Parent, Key = values[0], Color = Color.Green });
                                break;
                            default:
                                EnvironmentContext.Current.PresetMap.Add(values[0], Color.Blue);
                                Parent.Presets.Add(new Preset() { Parent = Parent, Key = values[0], Color = Color.Blue });
                                break;
                        }
                    }
                });
            }
        }

        private void isDark_Toggled(object sender, ToggledEventArgs e)
        {
            EnvironmentContext.Current.IsDarkTheme = e.Value;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (defaultColor.SelectedIndex == -1)
            {
                return;
            }
            var val = DefaultColors[defaultColor.SelectedIndex];
            var res = ColorMap[val];
            EnvironmentContext.Current.DefaultTextColor = Color.FromRgba(res.R, res.B, res.G, res.A);
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            EnvironmentContext.Current.ConnectionLimit = int.Parse(connLimit.Text);
        }

        private void Entry_Completed_1(object sender, EventArgs e)
        {
            EnvironmentContext.Current.DnsTimeout = int.Parse(dnsTimeout.Text);
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            EnvironmentContext.Current.LastMessagesCount = Convert.ToByte(e.NewValue);
        }

        private void Picker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            EnvironmentContext.Current.ConnectionConstraint = ConnectionConstraints[connConstraint.SelectedIndex];
        }

        private void check_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            EnvironmentContext.Current.UseAnimations = e.Value;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await EnvironmentContext.Current.SaveAsync();
        }

        private void topRemote_Completed(object sender, EventArgs e)
        {
            EnvironmentContext.Current.TopRemote = int.Parse(topRemote.Text);
        }
        private void culture_SelectedIndexChanged(object sender, EventArgs e)
        {
            Resource.Culture = new CultureInfo(CultureMap[SupportedCultures[culture.SelectedIndex]]);
            EnvironmentContext.Current.DefaultCulture = Resource.Culture.Name;
        }

        private async void chatImage_Clicked(object sender, EventArgs e)
        {
            await SelectImage(chatImage);
            EnvironmentContext.Current.ChatImageFileName = (chatImage.Source as FileImageSource).File;
        }
    }
}