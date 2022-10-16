using System;
using System.Collections.Generic;
using System.Text;
using ArchiNET.Constants;
namespace ArchiNET.Models
{
    public sealed class LocalStore
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconFileName { get; set; }
        public string Code { get; set; }
        public byte[] IconData { get; set; }
        public PatternType Type { get; set; }
        public bool HasPublish { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
