using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    public static class AnalyzerFactory
    { 
        public static DataAnalyzer CreateInstance(List<string> parsedText, int rowNumber)
        {
            switch (DataAnalyzer.ReadAction(parsedText))
            {
                case Actions.Properties:
                    return new Action0Analyzer(parsedText, rowNumber);
                case Actions.Labels:
                    return new Action4Analyzer(parsedText, rowNumber);
                //case Actions.Headers:
                //    return new Action8Analyzer(parsedText, rowNumber);
                default:
                    return null;
            }
        }
    }
}
