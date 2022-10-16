using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace ArchiNET.Services
{
    public interface IColorConverter
    {
        string ToFormat(Color color);
        Color ToColor(string format);
    }
}
