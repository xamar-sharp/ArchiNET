using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace ArchiNET.Services
{
    public sealed class CodeAnalysis : ITextAnalysis<Label>
    {
        public IDictionary<string, Color> PresetMap { get; }
        public Color Default { get; private set; }
        public bool LazyLoading { get; private set; }
        public CodeAnalysis(IDictionary<string, Color> presets, Color color)
        {
            PresetMap = presets;
            PresetMap.All(pair => { string.Intern(pair.Key); return true; });
            Default = color;
        }
        public ITextAnalysis<Label> AddPreset(string token, Color color)
        {
            PresetMap.Add(token, color);
            return this;
        }
        public ITextAnalysis<Label> RemovePreset(string token)
        {
            PresetMap.Remove(token);
            return this;
        }
        public ITextAnalysis<Label> SetLazyLoading(bool val)
        {
            LazyLoading = val;
            return this;
        }
        public void Analyse(string txt, Label label)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                throw new ArgumentNullException();
            }
            Span last = new Span();
            label.FormattedText = new FormattedString();
            for (int x = 0; x < txt.Length; x++)
            {
                if (Char.IsWhiteSpace(txt[x]))
                {
                    if (last.Text != String.Empty)
                    {
                        last.TextColor = Default;
                        label.FormattedText.Spans.Add(last);
                    }
                    last = new Span() { Text = String.Empty,TextColor=Default };
                    int counter = 1;
                    while (x + 1 < txt.Length && Char.IsWhiteSpace(txt[x + 1]))
                    {
                        x++;
                        counter++;
                    }
                    var res = String.Empty;
                    for (int y = 0; y < counter; y++)
                    {
                        res += " ";
                    }
                    label.FormattedText.Spans.Add(new Span() { TextColor = Default, Text = res });
                }
                else
                {
                    last.Text += txt[x];
                    if (PresetMap.ContainsKey(last.Text))
                    {
                        last.TextColor = PresetMap[last.Text];
                        label.FormattedText.Spans.Add(last);
                        last = new Span() { Text = String.Empty };
                    }
                    else
                    {
                        if (txt[x] == ';' || txt[x] == '{' || txt[x] == '}')
                        {
                            label.FormattedText.Spans.Add(new Span() { TextColor = Default, Text = "\n" });
                        }
                        string.Intern(last.Text);
                    }
                }
            }
            if (last.Text != String.Empty)
            {
                last.TextColor = Default;
                label.FormattedText.Spans.Add(last);
            }
        }
        public async Task AnalyseAsync(string txt, Label label)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                throw new ArgumentNullException();
            }
            Span last = new Span();
            label.FormattedText = new FormattedString();
            for (int x = 0;x<txt.Length;x++)
            {
                if (Char.IsWhiteSpace(txt[x]))
                {
                    if(last.Text != String.Empty)
                    {
                        last.TextColor = Default;
                        label.FormattedText.Spans.Add(last);
                    }
                    last = new Span() { Text = String.Empty };
                    int counter = 1;
                    while (x+1 < txt.Length && Char.IsWhiteSpace(txt[x+1]))
                    {
                        x++;
                        counter++;
                    }
                    var res = String.Empty;
                    for(int y = 0; y < counter; y++)
                    {
                        res += " ";
                    }
                    label.FormattedText.Spans.Add(new Span() { TextColor = Color.Black,Text =res });
                }
                else
                {
                    last.Text += txt[x];
                    if (PresetMap.ContainsKey(last.Text))
                    {
                        last.TextColor = PresetMap[last.Text];
                        label.FormattedText.Spans.Add(last);
                        last = new Span() { Text = String.Empty };
                    }
                    else
                    {
                        if (txt[x] == ';' || txt[x] == '{' || txt[x] == '}')
                        {
                            label.FormattedText.Spans.Add(new Span() { TextColor = Color.Black, Text = "\n" });
                        }
                        string.Intern(last.Text);
                    }
                    if (LazyLoading)
                    {
                        await Task.Yield();
                    }
                }
            }
            if (last.Text != String.Empty)
            {
                last.TextColor = Default;
                label.FormattedText.Spans.Add(last);
            }
            await Task.Yield();
        }
    }
}
