using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
using Xamarin.Essentials;
using System.Linq;
using Newtonsoft.Json;
namespace ArchiNET.Services
{
    public sealed class LocalStoreProvider : ILocalStoreProvider
    {
        public LocalStore[] Provide()
        {
            return JsonConvert.DeserializeObject<LocalStore[]>(Preferences.Get("LocalStores", "[]"), EnvironmentContext.Current.SerializerSettings);
        }
        public LocalStore ProvideByTitle(string title)
        {
            return Provide().First(ent => ent.Title == title);
        }
        public void Add(LocalStore store)
        {
            var els = Provide().ToList();
            els.Add(store);
            Preferences.Set("LocalStores", JsonConvert.SerializeObject(els, EnvironmentContext.Current.SerializerSettings));
        }
        public bool Remove(LocalStore store)
        {
            var els = Provide().ToList();
            bool res = els.Remove(store);
            Preferences.Set("LocalStores", JsonConvert.SerializeObject(els, EnvironmentContext.Current.SerializerSettings));
            return res;
        }
    }
}
