using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YGate.String.Operations
{
    public static class Trim
    {
        public static string CropStartEndMarkup(this string Data, string StartMarkup, string StopMarkup)
        {
            string returnedString = "";
            int StartValue = Data.IndexOf(StartMarkup);
            int StopValue  = Data.LastIndexOf(StopMarkup);
            returnedString = Data.Substring(StartValue, StopValue);
            return returnedString;
        }
    }
}
