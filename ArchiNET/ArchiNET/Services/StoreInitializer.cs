using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using ArchiNET.Models;
using System.Linq;
using System.IO;
using ArchiNET.Constants;
namespace ArchiNET.Services
{
    public static class StoreInitializer
    {
        public static bool Initialized => Preferences.ContainsKey("SystemStores");
        public static void Init()
        {
            if (Initialized)
            {
                Preferences.Remove("SystemStores");
            }
            ImageSource[] sources = new ImageSource[]
            {
                "prototype.jpg",
                "mediator.jpg",
                "compositor.jpg",
                "rest.jpg"
            };
            var paths = sources.Select(ent => (ent as FileImageSource).File);
            LocalStore[] stores = new LocalStore[]
            {
                new LocalStore(){Title=Resource.PrototypeTitle,Code = EnvironmentContext.Current.Assets.ProvideCode("PrototypeCode"),
                Description = Resource.PrototypeDescription,CreatedAt=DateTime.UtcNow,Type = PatternType.Creative,HasPublish=false },
                new LocalStore(){Title=Resource.MediatorTitle,Code = EnvironmentContext.Current.Assets.ProvideCode("MediatorCode"),
                Description = Resource.MediatorDescription,CreatedAt=DateTime.UtcNow,Type = PatternType.Behavior,HasPublish=false },
                new LocalStore(){Title=Resource.CompositorTitle,Code = EnvironmentContext.Current.Assets.ProvideCode("CompositorCode"),
                Description = Resource.CompositorDescription,CreatedAt=DateTime.UtcNow,Type = PatternType.Struct,HasPublish=false },
                new LocalStore() {Title=Resource.RestTitle,Code = EnvironmentContext.Current.Assets.ProvideCode("RestExample"),
                Description = Resource.RestDescription,CreatedAt=DateTime.UtcNow,Type = PatternType.Principle,HasPublish=false }
            };
            Preferences.Set("SystemStores", JsonConvert.SerializeObject(paths.Zip(stores, (path, store) => new LocalStore() { IconFileName = path, Description = store.Description,HasPublish=false,Code=store.Code,CreatedAt=store.CreatedAt,Title=store.Title,Type = store.Type }),EnvironmentContext.Current.SerializerSettings));
        }
        public static LocalStore GetStore(int preferenceId)
        {
            if (!Initialized)
            {
                return null;
            }
            LocalStore[] locals = JsonConvert.DeserializeObject<LocalStore[]>(Preferences.Get("SystemStores", "[]"));
            return locals[preferenceId];
        }
    }
}
