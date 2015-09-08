using OpenTTDTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    /// <summary>
    /// Basic analysing of a row.
    /// </summary>
    public class DataAnalyzer
    {
        public List<string> ParsedText { get; private set; }

        public DataAnalyzer(List<string> parsedText)
        {
            ParsedText = parsedText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAnalyzer"></param>
        /// <returns>False if the row should be ignored</returns>
        public bool ReadLabel(int rowNumber)
        {
            var code = default(int);
            var label = default(string);

            var feature = ReadFeature();

            var language = ReadLanguage();
            if (language == Constants.DEFAULT_LANGUAGE && feature != null)
            {
                var codeAndLabel = ReadCodeAndLabel();
                code = codeAndLabel.Code;
                label = codeAndLabel.Label;
            }
            else
            {
                return false;
            }

            VehicleManager.Instance.SetProperty(rowNumber, code, feature.Value, Constants.PROPERTY_LABEL_CODE, label);
            return true;
        }

        public bool ReadProperties(int rowNumber)
        {
            var feature = ReadFeature();
            switch (feature)
            {
                case Features.Trains:
                    var analyzer = new TrainDataAnalyzer(ParsedText);
                    return analyzer.ReadTrain(rowNumber);
                default:
                    break;
            }

            return true;
        }

        #region Row individual value processing
        public virtual Actions? ReadAction()
        {
            return ReadEnum<Actions>(Constants.INDEX_ACTIONS);
        }

        public virtual Features? ReadFeature()
        {
            return ReadEnum<Features>(Constants.INDEX_FEATURES);
        }

        public virtual int ReadLanguage()
        {
            return ReadHexData(Constants.INDEX_LANGUAGE);
        }

        public virtual dynamic ReadCodeAndLabel(bool ignoreLabel = false)
        {
            var code = default(int);
            var label = default(string);
            if (!TryReadHexData(Constants.INDEX_IDENTIFIER_AND_LABEL, out code))
            {
                code = (int)ParsedText[Constants.INDEX_IDENTIFIER_AND_LABEL][0];

                if (!ignoreLabel)
                    label = ParsedText[Constants.INDEX_IDENTIFIER_AND_LABEL].Substring(1);
            }
            else
            {
                if (!ignoreLabel)
                    label = ParsedText[Constants.INDEX_LABEL];
            }
            return new { Code = code, Label = label };
        }

        protected T? ReadEnum<T>(int index) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException();

            var hexValue = ReadHexData(index);
            return Enum.IsDefined(typeof(T), hexValue) ? (T)(object)hexValue : (T?)null;
        }

        protected int ReadHexData(int index)
        {
            return IntHelper.ConvertFromHex(ParsedText[index]);
        }

        protected bool TryReadHexData(int index, out int result)
        {
            return IntHelper.TryConvertFromHex(ParsedText[index], out result);
        }
        #endregion
    }
}
