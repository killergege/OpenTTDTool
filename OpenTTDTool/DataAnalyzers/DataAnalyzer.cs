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
        protected Dictionary<string, string> AlreadyReadProperties { get; private set; }
        protected int ReadIndex { get; set; }

        protected bool Cleaned;
        private static readonly ILog log = LogManager.GetLogger(typeof(DataAnalyzer));

        public DataAnalyzer(List<string> parsedText, int rowNumber, int initialReadIndex = Constants.INDEX_CLEANABLE)
        {
            ParsedText = parsedText;
            RowNumber = rowNumber;

            Cleaned = false;
            AlreadyReadProperties = new Dictionary<string, string>();
            ReadIndex = initialReadIndex;

            CleanParsedText();
        }

        public abstract bool ProcessData();

        public abstract void CleanParsedText();

        #region Row individual value processing
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
            return ReadIntField(FieldSizes.ExtendedByte, nameof(ReadCode)).Value;
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

        protected string ReadStringField(FieldSizes fieldSize, string fieldName = null)
        {
            return ReadRawValue(fieldSize, fieldName);
        }

        protected int? ReadIntField(FieldSizes fieldSize, string fieldName = null)
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
                var firstByte = ReadIntField(FieldSizes.Byte);
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
                    var nbData = ReadIntField(FieldSizes.Byte);
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
