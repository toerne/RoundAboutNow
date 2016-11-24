using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models
{
    public static class KeyKeeper
    {
        static string path = @"secrets\";
        
        public static string GetSLCloseByStationsKey()
        {
            return File.ReadAllLines(path + "keySLApiNarliggandeHallplatser.txt")[0];
        }

        public static string GetSLDisturbanceKey()
        {
            return File.ReadAllLines(path + "keySLStorningar.txt")[0];
        }
    }
}
