using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
namespace ArchiNET.Dependencies
{
    public interface IPlatformCultureProvider
    {
        CultureInfo Culture { get; }
    }
}
