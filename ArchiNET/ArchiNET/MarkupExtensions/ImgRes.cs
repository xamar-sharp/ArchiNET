using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ArchiNET.MarkupExtensions
{
    [ContentProperty("Name")]
    public sealed class ImgRes:IMarkupExtension
    {
        public string Name { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            return ImageSource.FromResource($"ArchiNET.Images.{Name}");
        }
    }
}
