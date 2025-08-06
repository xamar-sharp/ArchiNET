using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.Models;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Essentials;
using System.Linq;
namespace ArchiNET.Services
{
    public static class RestService
    {
        internal static readonly string IP = "localhost";
        internal static readonly HttpClient _client;
        internal static readonly IUserProvider _userProvider;
        internal static readonly IAuthorizationProvider _authProvider;
        static RestService()
        {
            string connectionString = $"http://{IP}:5000/";
            _userProvider = new UserProvider();
            _authProvider = new AuthorizationProvider();
            ServicePointManager.DefaultConnectionLimit = EnvironmentContext.Current.ConnectionLimit;
            ServicePointManager.DnsRefreshTimeout = EnvironmentContext.Current.DnsTimeout;
            _client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate, AllowAutoRedirect = true }) { MaxResponseContentBufferSize = int.MaxValue, Timeout = TimeSpan.FromMinutes(6), BaseAddress = new Uri(connectionString) };
        }
        internal static async Task SendLogsAsync(string log)
        {
            EnvironmentContext ctx = EnvironmentContext.Current;
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                _client.DefaultRequestHeaders.Add("Log", log);
                await _client.GetAsync("log");
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
            }
        }
        private static async Task RestoreAuth()
        {
            var auth = await _authProvider.Provide();
            if (auth.JwtExpires <= DateTime.UtcNow)
            {
                await RefreshAsync(auth.Refresh);
            }
        }
        internal static async Task SetupHeaders()
        {
            var auth = await _authProvider.Provide();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Jwt);
        }
        internal static async Task<bool> AuthorizeAsync(UserProfile profile = null)
        {
            if (profile is null)
            {
                var auth = await _authProvider.Provide();
                return await RefreshAsync(auth.Refresh) != null;
            }
            else if (profile != null)
            {
                profile.IconData = File.ReadAllBytes(profile.IconFileName);
                if (!string.IsNullOrEmpty(profile.IconFileName))
                {
                    var auth = await RegisterAsync(profile);
                    if (auth != null)
                    {
                        return await _userProvider.TrySave(profile);
                    }
                }
            }
            return false;
        }
        private static async Task<AuthorizationInfo> RefreshAsync(string refresh)
        {
            try
            {
                _client.DefaultRequestHeaders.Add("RefreshToken", refresh);
                var msg = await _client.GetAsync($"authorization", HttpCompletionOption.ResponseContentRead);
                msg.EnsureSuccessStatusCode();
                var auth = JsonConvert.DeserializeObject<AuthorizationInfo>(await msg.Content.ReadAsStringAsync());
                await _authProvider.TrySave(auth);
                return auth;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return null;
            }
        }
        private static async Task<AuthorizationInfo> RegisterAsync(UserProfile profile)
        {
            try
            {
                profile.CreatedAt = DateTime.UtcNow;
                var msg = await _client.PostAsync("authorization", new StringContent(JsonConvert.SerializeObject(profile, EnvironmentContext.Current.SerializerSettings), Encoding.UTF8, "application/json"));
                msg.EnsureSuccessStatusCode();
                var json = await msg.Content.ReadAsStringAsync();
                var auth = JsonConvert.DeserializeObject<AuthorizationInfo>(json, EnvironmentContext.Current.SerializerSettings);
                await _authProvider.TrySave(auth);
                return auth;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return null;
            }
        }
        internal static async Task<bool> UploadStore(LocalStore store)
        {
            try
            {
                store.IconData = File.ReadAllBytes(store.IconFileName);
                await RestoreAuth();
                await SetupHeaders();
                var dto = new RemoteStore() { Code =store.Code, Description = store.Description, IconData = File.ReadAllBytes(store.IconFileName), CreatedAt = DateTime.UtcNow, Title = store.Title, Type = store.Type };
                var msg = await _client.PostAsync("store", new StringContent(JsonConvert.SerializeObject(dto, EnvironmentContext.Current.SerializerSettings), Encoding.UTF8, "application/json"));
                return msg.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
        internal static async Task SignOutAsync()
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.DeleteAsync("authorization");
                _userProvider.Clear();
                _authProvider.Clear();
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
            }
        }
        internal static async Task<bool> EditUser(UserProfile edited)
        {
            try
            {
                edited.IconData = File.ReadAllBytes(edited.IconFileName);
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.PutAsync("authorization", new StringContent(JsonConvert.SerializeObject(edited, EnvironmentContext.Current.SerializerSettings), Encoding.UTF8, "application/json"));
                if (msg.IsSuccessStatusCode)
                {
                    await _authProvider.TrySave(JsonConvert.DeserializeObject<AuthorizationInfo>(await msg.Content.ReadAsStringAsync(), EnvironmentContext.Current.SerializerSettings));
                    await _userProvider.TrySave(edited);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
        internal static async Task<bool> DeleteStore(string title)
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                return (await _client.DeleteAsync($"store/{title}")).IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
        internal static async Task<RemoteStore> GetStore(string title)
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.GetAsync($"store/{title}");
                if (msg.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<RemoteStore>(await msg.Content.ReadAsStringAsync(), EnvironmentContext.Current.SerializerSettings);
                }
                return null;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return null;
            }
        }
        internal static async Task<IList<RemoteStore>> GetStores(int top, int offset)
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.DeleteAsync($"store/{top}/{offset}");
                if (msg.IsSuccessStatusCode)
                {
                    return EnvironmentContext.Current.SortFilter.Sort(JsonConvert.DeserializeObject<RemoteStore[]>(await msg.Content.ReadAsStringAsync(), EnvironmentContext.Current.SerializerSettings).ToList());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return null;
            }
        }
        internal static async Task<bool> SendMessage(string message)
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.PostAsync($"message",new StringContent(JsonConvert.SerializeObject(new Message() { CreatedAt=DateTime.UtcNow,Text=message},EnvironmentContext.Current.SerializerSettings),Encoding.UTF8,"application/json"));
                return msg.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
        internal static async Task<bool> DeleteMessage(DateTime createdAt,string text)
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.DeleteAsync($"message/{createdAt}/{text}");
                return msg.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
        internal static async Task<IList<Message>> GetMessages()
        {
            try
            {
                await RestoreAuth();
                await SetupHeaders();
                var msg = await _client.GetAsync($"message/{EnvironmentContext.Current.LastMessagesCount}");
                if (msg.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Message[]>(await msg.Content.ReadAsStringAsync(), EnvironmentContext.Current.SerializerSettings).ToList();
                }
                else
                {
                    return Enumerable.Empty<Message>().ToList();
                }
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return Enumerable.Empty<Message>().ToList();
            }
        }
        internal static async Task<UserProfile> SearchUser(string name)
        {
            try
            {
                return JsonConvert.DeserializeObject<UserProfile>(await (await _client.GetAsync($"authorization/{name}")).Content.ReadAsStringAsync(), EnvironmentContext.Current.SerializerSettings);
            }
            catch (Exception ex)
            {
                _client.CancelPendingRequests();
                return _userProvider.Provide();
            }
        }
        internal static Uri GetRealURI(string incomingURI,bool isStore)
        {
            return new Uri($"http://{IP}:5000/file?targetPath=" + incomingURI);
        }
    }
}
