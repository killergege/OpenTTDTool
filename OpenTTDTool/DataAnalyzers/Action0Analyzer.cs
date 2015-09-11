using OpenTTDTool.Entities;
using OpenTTDTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    /// <summary>
    /// Specs : http://newgrf-specs.tt-wiki.net/wiki/Action0
    /// </summary>
    public class Action0Analyzer : PseudoSpriteAnalyzer
    {
        public Action0Analyzer(List<string> parsedText, int rowNumber) : base(parsedText, rowNumber)
        {

        }

        public override bool ProcessData()
        {
            //Read data 
            var numProps = ReadIntFieldFromHex(FieldSizes.Byte);
            var numInfos = ReadIntFieldFromHex(FieldSizes.Byte);
            var code = ReadCode();
            return ReadProperties(numInfos.Value, numProps.Value);
        }

        public bool ReadProperties(int nbVehicles, int nbProperties)
        {
            var feature = ReadFeature();

            //TODO : Pour le moment on ne traite pas les modifications de plusieurs véhicules
            if (nbVehicles != 1 || !feature.HasValue)
            {
                return false;
            }

            var code = ReadCode();

            for (int i = 0; i < nbProperties; i++)
            {
                var propCode = ReadIntFieldFromHex(FieldSizes.Byte);

                var datalength = Vehicle.GetDataLength(propCode.Value, feature);
                if (datalength.HasValue)
                {
                    var intValue = ReadIntFieldFromHex(datalength.Value);
                    if (intValue.HasValue)
                        VehicleManager.Instance.SetProperty(RowNumber, code, feature.Value, propCode.Value, intValue);
                }
            }
            return true;
        }

        public override void CleanParsedText()
        {
            if (Cleaned)
                return;

            var cleanParsedText = new List<string>();

            for (int i = 0; i < ParsedText.Count; i++)
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

            Cleaned = true;
        }
    }
}
