using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Models;
using System.Threading.Tasks;
namespace ArchiNET.Services
{
    public interface IUserProvider
    {
        bool CanProvide { get; }
        UserProfile Provide();
        Task<bool> TrySave(UserProfile profile);
        void Clear();
    }
}
