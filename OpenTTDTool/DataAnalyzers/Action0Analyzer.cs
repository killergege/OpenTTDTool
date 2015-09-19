using OpenTTDTool.Entities;
using OpenTTDTool.Entities.GameEntities;
using OpenTTDTool.Helpers;
using OpenTTDTool.Managers;
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

        public override void Clean()
        {
            TextToHex(Constants.INDEX_CLEANABLE);
        }
    }
}
