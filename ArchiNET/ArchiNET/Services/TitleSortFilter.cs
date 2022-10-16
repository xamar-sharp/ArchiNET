using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
using System.Linq;
namespace ArchiNET.Services
{
    public sealed class TitleSortFilter : IStoreFilter
    {
        public IList<RemoteStore> Sort(IList<RemoteStore> stores)
        {
            return stores.OrderBy(ent => ent.Title).ToList();
        }
    }
}
