using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Constants;
namespace ArchiNET.Services
{
    public sealed class Logging:ILogging
    {
        public IList<string> UnsendedLogs { get; }
        private readonly IConnectionManager _connectionManager = new ConnectionManager();
        public async void Log(LogLevel level, string message)
        {
            var log = $"[{DateTime.UtcNow}]_{level}:{message}{Environment.NewLine}";
            if (_connectionManager.HasInternet)
            {
                UnsendedLogs.Add(log);
            }
            else
            {
                await RestService.SendLogsAsync(log);
                await SendUnsendedLogs();
            }
        }
        public async System.Threading.Tasks.Task SendUnsendedLogs()
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                while (_connectionManager.HasInternet && UnsendedLogs.Count > 0)
                {
                    foreach (var unsended in UnsendedLogs)
                    {
                        await RestService.SendLogsAsync(unsended);
                        UnsendedLogs.Remove(unsended);
                    }
                }
            });
        }
        public void RegisterAutoSending()
        {
            _connectionManager.OnConnectionChanged(async (param) =>
            {
                if (_connectionManager.HasInternet)
                {
                    await SendUnsendedLogs();
                }
            }, 0);
        }
    }
}
