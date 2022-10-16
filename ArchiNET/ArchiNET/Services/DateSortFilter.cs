using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ArchiNET.Models;
namespace ArchiNET.Services
{
    public sealed class DateSortFilter :IStoreFilter
    {
        public IList<RemoteStore> Sort(IList<RemoteStore> stores)
        {
            return stores.OrderBy(ent => ent.CreatedAt).ToList();
        }
    }
}
