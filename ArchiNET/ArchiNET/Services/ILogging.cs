using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Constants;
namespace ArchiNET.Services
{
    public interface ILogging
    {
        IList<string> UnsendedLogs { get; }
        void Log(LogLevel level, string message);
        void RegisterAutoSending();
    }
}
