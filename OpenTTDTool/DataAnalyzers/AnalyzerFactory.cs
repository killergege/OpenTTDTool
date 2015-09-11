using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    public static class AnalyzerFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnalyzerFactory));

        public static DataAnalyzer CreateInstance(List<string> parsedText)
        {
            int RowNumber = DataAnalyzer.ReadRowNumber(parsedText);

            switch (DataAnalyzer.ReadLineType(parsedText))
            {
                case LineType.RealSprite:
                    //Ignoré
                    return null;
                case LineType.Sound:
                    //Ignoré
                    return null;
                case LineType.PseudoSprite:
                    int size = DataAnalyzer.ReadLineSize(parsedText);
                    switch (PseudoSpriteAnalyzer.ReadAction(parsedText))
                    {
                        case Actions.Properties:
                            return new Action0Analyzer(parsedText, RowNumber);
                        case Actions.Labels:
                            return new Action4Analyzer(parsedText, RowNumber);
                        //case Actions.Headers:
                        //    return new Action8Analyzer(parsedText, rowNumber);
                        default:
                            return null;
                    }
                default:
                    log.Error($"Row {RowNumber}: Unknow line type \"{string.Join(",", parsedText)}\"");
                    return null;
            }

            
        }
    }
}
