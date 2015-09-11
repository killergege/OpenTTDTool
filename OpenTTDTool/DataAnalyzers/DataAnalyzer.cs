using log4net;
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
    }
}
