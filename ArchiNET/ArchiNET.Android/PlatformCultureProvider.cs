using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchiNET.Dependencies;
using Android.App;
using Android.Content;
using Android.OS;
using System.Globalization;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
namespace ArchiNET.Droid
{
    public sealed class PlatformCultureProvider:IPlatformCultureProvider
    {
        public CultureInfo Culture => new CultureInfo(Locale.Default.ToString().Replace("_", "-"));
    }
}