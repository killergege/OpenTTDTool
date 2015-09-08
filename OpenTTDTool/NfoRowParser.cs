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

            int numTemp;
            if (int.TryParse(ParsedText.FirstOrDefault(), out numTemp))
                Number = numTemp;

            if (ParsedText.Count > 1)
            {
                IsSprite = ParsedText[Constants.INDEX_SPRITES] != Constants.PROPERTY_SPRITE_NONE;
                if (IsSprite)
                {
                    Ignore = true;
                    return;
                }
            }

            var dataAnalyzer = new DataAnalyzer(ParsedText);
            switch (dataAnalyzer.ReadAction())
            {
                case Actions.Labels:
                    if (!dataAnalyzer.ReadLabel(Number))
                        Ignore = true;
                    break;

                case Actions.Properties:
                    if (!dataAnalyzer.ReadProperties(Number))
                        Ignore = true;
                    break;

                default:
                    Ignore = true;
                    break;
            }


        }
    }
}
