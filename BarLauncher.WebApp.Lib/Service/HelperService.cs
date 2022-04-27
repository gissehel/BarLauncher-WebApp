using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarLauncher.WebApp.Lib.Core.Service;

namespace BarLauncher.WebApp.Lib.Service
{
    public class HelperService : IHelperService
    {
        public bool ExtractProfile(string value, ref string newValue, ref string profile)
        {
            if (value.Contains("["))
            {
                var profileStart = value.IndexOf("[") + 1;
                var profileEnd = value.IndexOf("]", profileStart);
                if (profileStart > 0)
                {
                    if (profileEnd > profileStart)
                    {
                        profile = value.Substring(profileStart, profileEnd - profileStart);
                        newValue = value.Substring(0, profileStart - 1).TrimEnd(' ');
                        return true;
                    }
                    else if (profileEnd == -1)
                    {
                        profile = value.Substring(profileStart, value.Length - profileStart);
                        newValue = value.Substring(0, profileStart - 1).TrimEnd(' ');
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
