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
            var result = new List<string>();
            var currentWord = new StringBuilder();
            var isInString = false;
            var isEscaped = false;
            foreach(var car in myString)
            {
                if (isEscaped)
                {
                    currentWord.Append(car);
                    isEscaped = false;
                    continue;
                }
            
                if(car == '\\')
                {
                    isEscaped = true;
                    continue;
                }
                
                if(car == '\"')
                {
                    if (isInString)
                        isInString = false;
                    else
                        isInString = true;
                    continue;
                }

                if (!isInString && separators.Contains(car.ToString()))
                {
                    if (currentWord.Length != 0)
                        result.Add(currentWord.ToString());
                    currentWord.Clear();
                    isInString = false;
                }
                else
                {
                    currentWord.Append(car);
                }
            }
            if (currentWord.Length != 0)
                result.Add(currentWord.ToString());

            return result;
        }
    }
}
