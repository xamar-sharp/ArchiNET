using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
namespace ArchiNET.MarkupExtensions
{
    [ContentProperty("Name")]
    public sealed class Localizer:IMarkupExtension
    {
        public string Name { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            return Resource.ResourceManager.GetString(Name);
        }
    }
}
