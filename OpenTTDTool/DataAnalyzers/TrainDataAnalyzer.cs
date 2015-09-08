using OpenTTDTool.Entities;
using OpenTTDTool.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.DataAnalyzers
{
    public class TrainDataAnalyzer : DataAnalyzer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TrainDataAnalyzer));

        public TrainDataAnalyzer(List<string> parsedText) : base(parsedText)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the row should be ignored</returns>
        public bool ReadTrain(int rowNumber)
        {
            var nbVehicle = ReadHexData(Constants.DATA_TRAIN_INDEX_NB_VEHICLES);
            //TODO : Pour le moment on ne traite pas les modifications de plusieurs véhicules
            if (nbVehicle != 1)
            {
                return false;
            }

            var code = ReadCodeAndLabel(true).Code;

            var index = Constants.DATA_TRAIN_INDEX_PROPERTIES;
            while (index < ParsedText.Count)
            {
                var propCode = ReadHexData(index);

                var dataLength = Vehicle.GetDataLength(propCode);
                if (dataLength.HasValue && dataLength != Constants.PROPERTY_SPECIAL_LENGTH_VALUE)
                {
                    var textValue = String.Empty;
                    var intValue = default(int);
                    for (var i = index + dataLength.Value; i > index; i--)
                    {
                        if (i >= ParsedText.Count)
                        {
                            log.Error($"Row {rowNumber}: index would be out of range (index: {i}, Parsed text length: {ParsedText.Count}).");
                            return false;
                        }
                        textValue = string.Concat(textValue, ParsedText[i]);
                    }
                    if (IntHelper.TryConvertFromHex(textValue, out intValue))
                    {
                        VehicleManager.Instance.SetProperty(rowNumber, code, Features.Trains, propCode, intValue);
                    }
                }
                else if (dataLength != Constants.PROPERTY_SPECIAL_LENGTH_VALUE)
                {
                    //TODO : pas traité pour le moment. List of always refittable cargo types / never refittable cargo types
                    var nbData = ReadHexData(++index);
                    dataLength = nbData;
                }
                else
                {
                    return false;
                }
                index += dataLength.Value + 1;
            }
            return true;
        }
    }
}
