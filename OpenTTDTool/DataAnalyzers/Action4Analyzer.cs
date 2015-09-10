using OpenTTDTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    public class Action4Analyzer : DataAnalyzer
    {
        protected int Language { get; set; }

        public Action4Analyzer(List<string> parsedText, int rowNumber) : base(parsedText, rowNumber)
        {

        }

        public override bool ProcessData()
        {
            //Read data 
            Language = ReadIntField(FieldSizes.Byte).Value;
            var numEnt = ReadIntField(FieldSizes.Byte);

            return ReadLabel(Language);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the row should be ignored</returns>
        public bool ReadLabel(int language)
        {
            var feature = ReadFeature();
            var code = default(int);
            var label = default(string);

            if (language == Constants.DEFAULT_LANGUAGE && feature.HasValue)
            {
                code = ReadCode();
                label = ReadStringField(FieldSizes.String);
            }
            else
            {
                return false;
            }

            VehicleManager.Instance.SetProperty(RowNumber, code, feature.Value, Constants.PROPERTY_LABEL_CODE, label);
            return true;
        }

        public override void CleanParsedText()
        {
            if (Cleaned)
                return;

            int label_code;
            //if (Language == Constants.DEFAULT_LANGUAGE)
            //{
                if (!TryReadHexData(Constants.INDEX_IDENTIFIER, out label_code))
                {
                    Encoding enc = Encoding.GetEncoding(Constants.CODE_PAGE_NFO);
                    string cleanText = ParsedText[Constants.INDEX_IDENTIFIER].Substring(1);
                    string encodedValue = IntHelper.ConvertToHex(enc.GetBytes(ParsedText[Constants.INDEX_IDENTIFIER])[0]);
                    ParsedText[Constants.INDEX_IDENTIFIER] = cleanText;
                    ParsedText.Insert(Constants.INDEX_IDENTIFIER, encodedValue);
                }
            //}
        }
    }
}
