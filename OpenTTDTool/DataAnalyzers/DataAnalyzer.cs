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
        private bool cleaned;

        public List<string> ParsedText { get; private set; }

        public DataAnalyzer(List<string> parsedText)
        {
            ParsedText = parsedText;
            cleaned = false;
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
            Actions? action = ReadEnum<Actions>(Constants.INDEX_ACTIONS);
            cleanParsedText(action);
            return action;
        }

        public virtual Features? ReadFeature()
        {
            return ReadEnum<Features>(Constants.INDEX_FEATURES);
        }

        private void cleanParsedText(Actions? action)
        {
            if (cleaned)
                return;

            List<string> cleanParsedText;
            switch (action)
            {
                case Actions.Labels:
                    int label_code;
                    if (ReadLanguage() == Constants.DEFAULT_LANGUAGE)
                    {
                        if (!TryReadHexData(Constants.INDEX_IDENTIFIER_AND_LABEL, out label_code))
                        {
                            Encoding enc = Encoding.GetEncoding(Constants.CODE_PAGE_NFO);
                            string cleanText = ParsedText[Constants.INDEX_IDENTIFIER_AND_LABEL].Substring(1);
                            string encodedValue = IntHelper.ConvertToHex(enc.GetBytes(ParsedText[Constants.INDEX_IDENTIFIER_AND_LABEL])[0]);
                            ParsedText[Constants.INDEX_IDENTIFIER_AND_LABEL] = cleanText;
                            ParsedText.Insert(Constants.INDEX_IDENTIFIER_AND_LABEL, encodedValue);
                            
    }
                    }
                    break;
                case Actions.Properties:
                    cleanParsedText = new List<string>();
                    
                    for(int i = 0;i<ParsedText.Count;i++)
                    {
                        //On vérifie les valeurs à partir de la 5ième car on ignore : la ligne, le sprite, le nb d'élement, l'action
                        if (i < Constants.INDEX_CLEANABLE)
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
                    break;
                default:
                    break;
            }
            cleaned = true;
        }

        public virtual int ReadLanguage()
        {
            return ReadHexData(Constants.INDEX_LANGUAGE);
        }

        public virtual dynamic ReadCodeAndLabel(bool ignoreLabel = false)
        {
            var code = default(int);
            var label = default(string);
            code = ReadHexData(Constants.INDEX_IDENTIFIER);
            if(!ignoreLabel)
            {
                cleanParsedText(Actions.Labels);
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
