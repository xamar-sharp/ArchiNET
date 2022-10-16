using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiNET.Models
{
    public sealed class UserProfile
    {
        public string Login { get; set; }
        public string Description { get; set; }
        public string IconFileName { get; set; }
        public byte[] IconData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
