using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarLauncher.WebApp.Lib.Core.Service
{
    public interface IHelperService
    {
        bool ExtractProfile(string value, ref string newValue, ref string profile);
    }
}
