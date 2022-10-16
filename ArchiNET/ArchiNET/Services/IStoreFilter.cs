using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
namespace ArchiNET.Services
{
    public interface IStoreFilter
    {
       IList<RemoteStore> Sort(IList<RemoteStore> stores);
    }
}
