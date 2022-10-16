using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ArchiNET.Models;
using Newtonsoft.Json;
namespace ArchiNET.Services
{
    public sealed class UserProvider : IUserProvider
    {
        public bool CanProvide => Preferences.ContainsKey("LocalUser");
        public UserProfile Provide()
        {
            if (!CanProvide)
            {
                return null;
            }
            var val = JsonConvert.DeserializeObject<UserProfile>(Preferences.Get("LocalUser", "{}"), EnvironmentContext.Current.SerializerSettings);
            return val;
        }
        public async Task<bool> TrySave(UserProfile profile)
        {
            try
            {
                Preferences.Set("LocalUser", JsonConvert.SerializeObject(profile, EnvironmentContext.Current.SerializerSettings));
                await Task.Yield();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Clear()
        {
            Preferences.Remove("LocalUser");
        }
    }
}
