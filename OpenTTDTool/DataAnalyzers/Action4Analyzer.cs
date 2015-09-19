using OpenTTDTool.Entities.SupportEntities;
using OpenTTDTool.Helpers;
using OpenTTDTool.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    public class Action4Analyzer : PseudoSpriteAnalyzer
    {
        public Action4Analyzer(List<string> parsedText, int rowNumber) : base(parsedText, rowNumber)
        {

        }

        public override bool ProcessData()
        {
            //Read data 
            var language = ReadIntFieldFromHex(FieldSizes.Byte).Value;
            var extended = false;
            if ((language & 0x80) == 0x80)
            {
                PositionToHex(Constants.INDEX_EXTENDED_IDENTIFIER);
                extended = true;
            }

            //TODO : lire toutes les chaines en fonction du nombre d'entité
            var numEnt = ReadIntFieldFromHex(FieldSizes.Byte);

            return ReadLabel(language, extended);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the row should be ignored</returns>
        public bool ReadLabel(int language, bool extended)
        {
            var feature = ReadFeature();
            var code = default(int);
            var label = default(string);

            if (feature.HasValue)
            {
                if (extended)
                    code = ReadCode(FieldSizes.Word);
                else
                    code = ReadCode();
                label = ReadStringField(FieldSizes.String);
            }
            else
            {
                return false;
            }

            VehicleManager.Instance.SetProperty(RowNumber, code, feature.Value, Constants.PROPERTY_LABEL_CODE, new LocalizedString(language, label));
            return true;
        }

        public override void Clean()
        {
            PositionToHex(Constants.INDEX_IDENTIFIER);
        }
    }
}
