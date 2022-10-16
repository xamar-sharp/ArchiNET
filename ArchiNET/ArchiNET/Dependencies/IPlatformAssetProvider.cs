using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiNET.Dependencies
{
    public interface IPlatformAssetProvider
    {
        string ProvideCode(string fileName);
    }
}
