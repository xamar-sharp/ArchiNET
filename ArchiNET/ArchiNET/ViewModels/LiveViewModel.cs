using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ArchiNET.ViewModels
{
    public class LiveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public virtual void UpdateState()
        {

        }
    }
}
