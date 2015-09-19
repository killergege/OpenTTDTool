using log4net;
using OpenTTDTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    /// <summary>
    /// Basic analysing of a PseudoSprite row.
    /// </summary>
    public abstract class PseudoSpriteAnalyzer : DataAnalyzer
    {

        public PseudoSpriteAnalyzer(List<string> parsedText, int rowNumber, int initialReadIndex = Constants.INDEX_CLEANABLE)
        {
            ParsedText = parsedText;
            RowNumber = rowNumber;

            AlreadyReadProperties = new Dictionary<string, string>();
            ReadIndex = initialReadIndex;

            Clean();
        }

        public abstract void Clean();

        public static Actions? ReadAction(List<string> parsedText)
        {
            var hexValue = IntHelper.ConvertFromHex(parsedText[Constants.INDEX_ACTIONS]);
            return Enum.IsDefined(typeof(Actions), hexValue) ? (Actions)hexValue : (Actions?)null;
        }
 

        public virtual Features? ReadFeature()
        {
            return ReadEnum<Features>(Constants.INDEX_FEATURES);
        }

        public virtual int ReadCode(FieldSizes size = FieldSizes.ExtendedByte)
        {
            return ReadIntFieldFromHex(size, nameof(ReadCode)).Value;
        }

        public void TextToHex(int startIndex)
        {
            var cleanParsedText = new List<string>();

            for (int i = 0; i < ParsedText.Count; i++)
            {
                //On vérifie les valeurs à partir de la 5ième car on ignore : la ligne, le sprite, le nb d'élement, l'action
                if (i < startIndex)
                {
                    cleanParsedText.Add(ParsedText[i]);
                }
                else
                {
                    int property_code;
                    //Si le texte n'est pas 2 charactères hexa, c'est sans doute une chaine à convertir
                    if (ParsedText[i].Length > 2 || !IntHelper.TryConvertFromHex(ParsedText[i], out property_code))
                    {
                        Encoding enc = Encoding.GetEncoding(Constants.CODE_PAGE_NFO);
                        enc.GetBytes(ParsedText[i]).ToList().ForEach(p => cleanParsedText.Add(IntHelper.ConvertToHex(p)));
                    }
                    else
                    {
                        cleanParsedText.Add(ParsedText[i]);
                    }
                }
            }
            ParsedText = cleanParsedText;
        }

        public void PositionToHex(int index)
        {
            int label_code;
            if (!TryReadHexData(index, out label_code))
            {
                Encoding enc = Encoding.GetEncoding(Constants.CODE_PAGE_NFO);
                string cleanText = ParsedText[index].Substring(1);
                string encodedValue = IntHelper.ConvertToHex(enc.GetBytes(ParsedText[index])[0]);
                ParsedText[index] = cleanText;
                ParsedText.Insert(index, encodedValue);
            }
        }
    }
}
