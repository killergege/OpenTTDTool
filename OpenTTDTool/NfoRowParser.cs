using OpenTTDTool.DataAnalyzers;
using OpenTTDTool.Entities;
using OpenTTDTool.Helpers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    public class NfoRowParser
    {
        public int Number { get; set; }
        public bool IsSprite { get; private set; }
        public Actions? Action { get; private set; }

        public bool Ignore { get; private set; }

        public List<string> ParsedText { get; set; }
        public string FullText { get; set; }

        public NfoRowParser(string ligne)
        {
            AddContent(ligne);
        }

        public void AddContent(string content)
        {
            FullText = String.Concat(FullText, content);
        }

        public void Parse()
        {
            ParsedText = FullText.SplitWithQuotes(" ", "\t");

            var analyzer = AnalyzerFactory.CreateInstance(ParsedText);
            if (analyzer == null)
            {
                Ignore = true;
            }
            else
            {
                var success = analyzer.ProcessData();
                if (!success)
                    Ignore = true;
            }
        }
    }
}
