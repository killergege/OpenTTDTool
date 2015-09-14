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
    /// Basic analysing of a row.
    /// </summary>
    public abstract class DataAnalyzer
    {
        public List<string> ParsedText { get; protected set; }
        public int RowNumber { get; set; }

        /// <summary>
        /// Reading properties moved the readindex, so if the property has already been read, it doesn't move
        /// </summary>
        protected Dictionary<string, string> AlreadyReadProperties { get; set; }
        protected int ReadIndex { get; set; }

        protected static readonly ILog log = LogManager.GetLogger(typeof(PseudoSpriteAnalyzer));

        public abstract bool ProcessData();

        public static int ReadRowNumber(List<string> parsedText)
        {
            try
            {
                return int.Parse(parsedText[Constants.INDEX_ROW]);
            }
            catch
            {
                log.Error($"Row unknown: Error parsing row number value \"{string.Join(",", parsedText)}\" on ReadIndex {Constants.INDEX_ROW}");
                throw;
            }
        }

        public static int ReadLineSize(List<string> parsedText, int? row = null)
        {
            try
            {
                return int.Parse(parsedText[Constants.INDEX_SIZE]);
            }
            catch
            {
                string rowNum = row == null ? "unkwknown" : row.ToString();
                log.Error($"Row {rowNum}: Error parsing row number value \"{string.Join(",", parsedText)}\" on ReadIndex {Constants.INDEX_SIZE}");
                throw;
            }
        }

        public static LineType? ReadLineType(List<string> parsedText)
        {
            string sprite = parsedText[Constants.INDEX_SPRITES];
            if (sprite.Equals(Constants.PROPERTY_SPRITE_NONE))
                return LineType.PseudoSprite;
            if (sprite.Equals(Constants.PROPERTY_SPRITE_SOUND))
                return LineType.Sound;
            return LineType.RealSprite;
        }

        #region Technical analyzers
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

        protected string ReadStringField(FieldSizes fieldSize, string fieldName = null)
        {
            return ReadRawValue(fieldSize, fieldName);
        }

        protected int? ReadIntLitteralField(FieldSizes fieldSize, string fieldName = null)
        {
            string raw = null;

            try
            {
                raw = ReadRawValue(fieldSize, fieldName);

                int intValue;
                if (int.TryParse(raw, out intValue))
                    return intValue;
                else
                    return null;
            }
            catch
            {
                log.Error($"Row {RowNumber}: Error parsing litteral int value \"{raw}\" on ReadIndex {ReadIndex}");
                throw;
            }
        }

        protected int? ReadIntFieldFromHex(FieldSizes fieldSize, string fieldName = null)
        {
            string raw = null;
            try
            {
                raw = ReadRawValue(fieldSize, fieldName);

                int intValue;
                if (IntHelper.TryConvertFromHex(raw, out intValue))
                    return intValue;
                else
                    return null;
            }
            catch
            {
                log.Error($"Row {RowNumber}: Error parsing int value \"{raw}\" on ReadIndex {ReadIndex}");
                throw;
            }
        }

        private string ReadRawValue(FieldSizes fieldSize, string fieldName = null)
        {
            var textValueBuilder = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(fieldName) && AlreadyReadProperties.ContainsKey(fieldName))
                return AlreadyReadProperties[fieldName];

            var length = Constants.FieldLengths[fieldSize];

            if (fieldSize == FieldSizes.ExtendedByte)
            {
                var firstByte = ReadIntFieldFromHex(FieldSizes.Byte);
                if (firstByte == 0xFF)
                {
                    length = 2;
                }
                else
                {
                    //Value already read
                    textValueBuilder = new StringBuilder(firstByte.ToString());
                    length = null;
                }
            }

            if (length.HasValue)
            {
                for (var i = (ReadIndex - 1) + length.Value; i >= ReadIndex; i--)
                {
                    if (i >= ParsedText.Count)
                    {
                        log.Error($"Row {RowNumber}: index would be out of range (index: {i}, Parsed text length: {ParsedText.Count}).");
                        return null;
                    }
                    textValueBuilder = textValueBuilder.Append(ParsedText[i]);
                }

            }
            else
            {
                if (fieldSize == FieldSizes.MultipleBytes)
                {
                    //TODO : pas traité pour le moment. List of always refittable cargo types / never refittable cargo types
                    var nbData = ReadIntFieldFromHex(FieldSizes.Byte);
                    length = nbData;
                }
            }

            if (length.HasValue)
                ReadIndex += length.Value;

            var textValue = textValueBuilder.ToString();
            if (!String.IsNullOrWhiteSpace(fieldName))
                AlreadyReadProperties.Add(fieldName, textValue);
            return textValue;
        }
        #endregion
    }
}
