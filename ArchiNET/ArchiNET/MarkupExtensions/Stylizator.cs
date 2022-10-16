using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using ArchiNET.Services;
namespace ArchiNET.MarkupExtensions
{
    [ContentProperty("View")]
    public sealed class Stylizator:IMarkupExtension
    {
        public object View { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            return View.GetType().GetMethod("SetDynamicResource").Invoke(View, new object[] { VisualElement.StyleProperty, $"{(EnvironmentContext.Current.IsDarkTheme ? "Night" : "Day")}{View.GetType().Name}" });
        }
    }
}
