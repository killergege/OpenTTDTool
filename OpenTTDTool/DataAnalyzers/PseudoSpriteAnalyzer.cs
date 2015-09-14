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

        protected bool Cleaned;

        public PseudoSpriteAnalyzer(List<string> parsedText, int rowNumber, int initialReadIndex = Constants.INDEX_CLEANABLE)
        {
            ParsedText = parsedText;
            RowNumber = rowNumber;

            Cleaned = false;
            AlreadyReadProperties = new Dictionary<string, string>();
            ReadIndex = initialReadIndex;

            CleanParsedText();
        }

        public abstract void CleanParsedText();

        public static Actions? ReadAction(List<string> parsedText)
        {
            var hexValue = IntHelper.ConvertFromHex(parsedText[Constants.INDEX_ACTIONS]);
            return Enum.IsDefined(typeof(Actions), hexValue) ? (Actions)hexValue : (Actions?)null;
        }
 

        public virtual Features? ReadFeature()
        {
            return ReadEnum<Features>(Constants.INDEX_FEATURES);
        }

        public virtual int ReadCode()
        {
            return ReadIntFieldFromHex(FieldSizes.ExtendedByte, nameof(ReadCode)).Value;
        }
    }
}
