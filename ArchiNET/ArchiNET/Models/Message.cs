using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiNET.Models
{
    public sealed class Message
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserIconUri { get; set; }
    }
}
