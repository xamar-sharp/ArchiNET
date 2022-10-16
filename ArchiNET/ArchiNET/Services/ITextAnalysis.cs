using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ArchiNET.Services
{
    public interface ITextAnalysis<T> where T : View
    {
        IDictionary<string, Color> PresetMap { get; }
        Color Default { get;}
        bool LazyLoading { get; }
        ITextAnalysis<T> SetLazyLoading(bool val);
        ITextAnalysis<T> AddPreset(string token, Color color);
        ITextAnalysis<T> RemovePreset(string token);
        void Analyse(string txt, T view);
        Task AnalyseAsync(string txt, T view);
    }
}
