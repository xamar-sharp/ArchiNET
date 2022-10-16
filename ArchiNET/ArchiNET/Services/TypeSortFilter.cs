using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ArchiNET.Models;
namespace ArchiNET.Services
{
    public sealed class TypeSortFilter : IStoreFilter
    {
        public IList<RemoteStore> Sort(IList<RemoteStore> stores)
        {
            return stores.OrderBy(ent => ent.Type.ToString()).ToList();
        }
    }
}
