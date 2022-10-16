using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Constants;
using System.Threading.Tasks;
using Xamarin.Forms;
using ArchiNET.Models;
using ArchiNET.Dependencies;
using ArchiNET.Services;
using System.Net;
using Xamarin.Essentials;
using Newtonsoft.Json;
namespace ArchiNET.Services
{
    public sealed class EnvironmentContext
    {
        private static readonly EnvironmentContext _singleTone;
        static EnvironmentContext()
        {
            _singleTone = new EnvironmentContext()
            {
                SortFilter = new DateSortFilter(),
                IsDarkTheme = true,
                DnsTimeout = ServicePointManager.DnsRefreshTimeout,
                ConnectionLimit = ServicePointManager.DefaultConnectionLimit,
                ConnectionConstraint = ConnectionType.Ethernet,
                UseAnimations = true,
                LastMessagesCount = 10,
                ChatImageFileName = (ImageSource.FromFile("chatBack.jpg") as FileImageSource).File,
                ChatSendImageFileName = (ImageSource.FromFile("chatSend.png") as FileImageSource).File,
                TextBarBackColor = Resource.Gray,
                ChatAvatarColor = Resource.White,
                ChatMessageColor = Resource.White,
                ChatDateColor = Resource.Green,
                RefreshColor = Resource.Green,
                TextBarColor = Resource.White
            };
        }
        public const string ExternalPath = "/storage/emulated/0/Android/data/com.companyname.ArchiNet/";
        public static EnvironmentContext Current => _singleTone;
        public UserProfile Profile { get => UserProvider.Provide(); }
        public EnvironmentContext()
        {
            SerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            UserProvider = new UserProvider();
            ConnectionMap = new Dictionary<ConnectionType, ConnectionProfile>(2)
            {
                [ConnectionType.Bluetooth] = ConnectionProfile.Bluetooth,
                [ConnectionType.Cellular] = ConnectionProfile.Cellular,
                [ConnectionType.Ethernet] = ConnectionProfile.Ethernet,
                [ConnectionType.WiFi] = ConnectionProfile.WiFi
            };
            Assets = DependencyService.Get<IPlatformAssetProvider>();
            PresetMap = new Dictionary<string, Color>(4)
            {
                [Resource.Association] = Color.CornflowerBlue,
                [Resource.Aggregation] = Color.OrangeRed,
                [Resource.Composition] = Color.Firebrick,
                [Resource.Generalization] = Color.ForestGreen,
                [Resource.Implementation] = Color.GreenYellow,
                [Resource.Interface] = Color.Yellow,
                [Resource.Class] = Color.Green,
                [Resource.Enum] = Color.Red,
                [Resource.Pattern] = Color.Gold,
                [Resource.Principle] = Color.RoyalBlue,
                [Resource.CSharp] = Color.CadetBlue,
                ["public"] = Color.Blue,
                ["private"] = Color.Blue,
                ["protected"] = Color.Gray,
                ["using"] = Color.ForestGreen,
                ["namespace"] = Color.Orange,
                ["class"] = Color.Blue,
                ["interface"] = Color.Blue,
                ["enum"] = Color.Blue
            };
            ColorConverter = new ColorConverter();
        }
        public IColorConverter ColorConverter { get; }
        public IPlatformAssetProvider Assets { get; }
        public Color DefaultTextColor { get; set; }
        public IUserProvider UserProvider { get; set; }
        public ITextAnalysis<Label> CodeAnalyser { get => new CodeAnalysis(PresetMap, DefaultTextColor); }
        public JsonSerializerSettings SerializerSettings { get; }
        public int ConnectionLimit { get; set; }
        public string DefaultCulture { get; set; }
        public bool HasBackup { get => Preferences.ContainsKey("Settings"); }
        public int DnsTimeout { get; set; }
        public int TopRemote { get; set; }
        public byte LastMessagesCount { get; set; }
        public string TextBarColor { get; set; }
        public string TextBarBackColor { get; set; }
        public string RefreshColor { get; set; }
        public string ChatDateColor { get; set; }
        public string ChatAvatarColor { get; set; }
        public string ChatMessageColor { get; set; }
        public string ChatSendImageFileName { get; set; }
        public string ChatImageFileName { get; set; }
        public ImageSource ChatSendImage { get => ImageSource.FromFile(ChatSendImageFileName); }
        public ImageSource ChatImage { get => ImageSource.FromFile(ChatImageFileName); }
        public IDictionary<ConnectionType, ConnectionProfile> ConnectionMap { get; }
        public IDictionary<string, Color> PresetMap { get; }
        public ConnectionType ConnectionConstraint { get; set; }
        public IStoreFilter SortFilter { get; internal set; }
        public bool UseAnimations { get; set; }
        public bool IsDarkTheme { get; set; }
        public EnvironmentContext SetSortFilter(SortType type)
        {
            switch (type)
            {
                case SortType.Date:
                    SortFilter = new DateSortFilter();
                    break;
                case SortType.Title:
                    SortFilter = new TitleSortFilter();
                    break;
                default:
                    SortFilter = new TypeSortFilter();
                    break;
            }
            return this;
        }
        public async Task SaveAsync()
        {
            await Task.Run(() =>
            {
                Preferences.Remove("Settings");
                string json = JsonConvert.SerializeObject(this, this.SerializerSettings);
                Preferences.Set("Settings", json);
            });
        }
        public async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                var ctx = JsonConvert.DeserializeObject<EnvironmentContext>(Preferences.Get("Settings", "{}"), SerializerSettings);
                Current.Setup(ctx);
            });
        }
        internal void SetupPlatformCulture()
        {
            Resource.Culture = DependencyService.Get<IPlatformCultureProvider>().Culture;
            DefaultCulture = Resource.Culture.Name;

        }
        internal async void SetupContext()
        {
            if (!HasBackup)
            {
                SetupPlatformCulture();
            }
            else
            {
                await LoadAsync();
            }
        }
        private void Setup(EnvironmentContext ctx)
        {
            Current.TextBarColor = ctx.TextBarColor;
            Current.TextBarBackColor = ctx.TextBarBackColor;
            Current.ChatDateColor = ctx.ChatDateColor;
            Current.ChatAvatarColor = ctx.ChatAvatarColor;
            Current.ChatMessageColor = ctx.ChatMessageColor;
            Current.RefreshColor = ctx.RefreshColor;
            Current.DefaultCulture = ctx.DefaultCulture;
            Current.ChatSendImageFileName = ctx.ChatSendImageFileName;
            Current.ChatImageFileName = ctx.ChatImageFileName;
            Resource.Culture = new System.Globalization.CultureInfo(Current.DefaultCulture);
            Current.DnsTimeout = ctx.DnsTimeout;
            Current.ConnectionConstraint = ctx.ConnectionConstraint;
            Current.ConnectionLimit = ctx.ConnectionLimit;
            Current.IsDarkTheme = ctx.IsDarkTheme;
            Current.DefaultTextColor = ctx.DefaultTextColor;
            Current.UseAnimations = ctx.UseAnimations;
            Current.TopRemote = TopRemote;
            Current.SetSortFilter(SortType.Date);
            Current.LastMessagesCount = ctx.LastMessagesCount;
        }
    }
}
