using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace ArchiNET.Services
{
    public sealed class ColorConverter:IColorConverter
    {
        public static readonly IDictionary<string, Color> map;
        static ColorConverter()
        {
            map = new Dictionary<string, Color>()
            {
                [Resource.Black] = Color.Black,
                [Resource.White] = Color.White,
                [Resource.Green] = Color.Green,
                [Resource.CornflowerBlue] = Color.CornflowerBlue,
                [Resource.Red] = Color.Red,
                [Resource.Gray] = Color.Gray,
                [Resource.MediumOrchid] = Color.MediumOrchid,
                [Resource.Orchid] = Color.DarkOrchid
            };
        }
        public Color ToColor(string format)
        {
            return map[format];
        }
        public string ToFormat(Color color)
        {
            return map.First(ent => ent.Value == color).Key;
        }
    }
}
