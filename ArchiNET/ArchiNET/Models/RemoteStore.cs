using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Constants;
namespace ArchiNET.Models
{
    public sealed class RemoteStore
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public byte[] IconData { get; set; }
        public string IconURI { get; set; }
        public PatternType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
