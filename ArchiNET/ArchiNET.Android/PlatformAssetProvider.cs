using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ArchiNET.Dependencies;

namespace ArchiNET.Droid
{
    public sealed class PlatformAssetProvider:IPlatformAssetProvider
    {
        internal static Android.Content.Res.AssetManager source;
        public string ProvideCode(string csName)
        {
            StreamReader reader = new StreamReader(source.Open(csName + ".cs"));
            try
            { 
                var value= reader.ReadToEnd();
                return value;
            }
            finally
            {
                reader.Dispose();
            }
        }
    }
}