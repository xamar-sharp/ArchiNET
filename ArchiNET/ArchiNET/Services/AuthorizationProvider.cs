using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using ArchiNET.Models;
using System.Threading.Tasks;
namespace ArchiNET.Services
{
    public sealed class AuthorizationProvider : IAuthorizationProvider
    {
        public async Task<AuthorizationInfo> Provide()
        {
            return await Task.Run(async () =>
            {
                var info = new AuthorizationInfo();
                info.Jwt = Preferences.Get("Jwt", "{}");
                info.JwtExpires = Preferences.Get("JwtExpires", DateTime.UtcNow);
                info.Refresh = await SecureStorage.GetAsync("Refresh");
                return info;
            }).ConfigureAwait(false);
        }
        public async Task<bool> TrySave(AuthorizationInfo info)
        {
            try
            {
                Preferences.Set("Jwt", info.Jwt);
                Preferences.Set("JwtExpires", info.JwtExpires);
                await SecureStorage.SetAsync("Refresh", info.Refresh);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Clear()
        {
            Preferences.Remove("Jwt");
            Preferences.Remove("JwtExpires");
            SecureStorage.RemoveAll();
        }
    }
}
