using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenTTDTool
{
    public static class StringExtension
    {
        public static List<string> SplitWithQuotes(this string myString, params string[] separators)
        {
            myString = myString.Replace("\"\\\"", "22 \"");
            myString = myString.Replace("\\\"", "'");
            return myString.Split('"')
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(separators, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToList();
        }
    }
}
