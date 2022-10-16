using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.Linq;
namespace ArchiNET.Services
{
    public sealed class ConnectionManager:IConnectionManager
    {
        public bool HasInternet => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public bool IsValid(EnvironmentContext ctx)
        {
            return Connectivity.ConnectionProfiles.All(ent=>EnvironmentContext.Current.ConnectionMap[ctx.ConnectionConstraint] != ent);
        }
        public void OnConnectionChanged(Action<object> action,object obj)
        {
            Connectivity.ConnectivityChanged += (sender, conn) => action(obj);
        }
    }
}
