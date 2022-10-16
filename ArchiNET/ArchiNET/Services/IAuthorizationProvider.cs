using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArchiNET.Models;
namespace ArchiNET.Services
{
    public interface IAuthorizationProvider
    {
        Task<AuthorizationInfo> Provide();
        Task<bool> TrySave(AuthorizationInfo info);
        void Clear();
    }
}
