using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
namespace ArchiNET.Services
{
    public interface ILocalStoreProvider
    {
        LocalStore[] Provide();
        LocalStore ProvideByTitle(string title);
        void Add(LocalStore store);
        bool Remove(LocalStore store);
    }
}
