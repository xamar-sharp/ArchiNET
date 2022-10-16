using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiNET.Models
{
    public sealed class AuthorizationInfo
    {
        public string Jwt { get; set; }
        public DateTime JwtExpires { get; set; }
        public string Refresh { get; set; }
    }
}
