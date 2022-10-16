using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiNET.Services
{
    public interface IConnectionManager
    {
        bool HasInternet { get; }
        bool IsValid(EnvironmentContext ctx);
        void OnConnectionChanged(Action<object> callback, object param);
    }
}
