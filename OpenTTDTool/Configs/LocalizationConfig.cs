using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Configs
{
    class LocalizationConfig
    {
        public enum SpeedUnit
        {
            [Description("Imperial (mph)")]
            Imperial,
            [Description("Metric (km/h)")]
            Metric,
            [Description("SI (m/s)")]
            SI
        }

        private static LocalizationConfig instance = null;
        private SpeedUnit rawUnit = SpeedUnit.Imperial;

        public SpeedUnit displayUnit { get; set; }

        private LocalizationConfig() {

        }

        public static LocalizationConfig getInstance()
        {
            if (instance == null)
                instance = new LocalizationConfig();
            return instance;
        }

        public int Convert(int value)
        {
            return Convert(value, rawUnit, displayUnit);
        }

        public int Convert(int value, SpeedUnit targetUnit)
        {
            return Convert(value, rawUnit, targetUnit);
        }

        public int Convert(int value, SpeedUnit sourceUnit, SpeedUnit targetUnit)
        {
            if (sourceUnit == targetUnit)
                return value;

            double sourceValue = value;
            double pivotValue;
            double targetValue;

            //Pivot : SI
            switch (sourceUnit)
            {
                case SpeedUnit.Imperial:
                    pivotValue = sourceValue / 2.23693629205;
                    break;
                case SpeedUnit.Metric:
                    pivotValue = sourceValue / 3.6;
                    break;
                case SpeedUnit.SI:
                    pivotValue = sourceValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (targetUnit)
            {
                case SpeedUnit.Imperial:
                    targetValue = pivotValue * 2.23693629205;
                    break;
                case SpeedUnit.Metric:
                    targetValue = pivotValue * 3.6;
                    break;
                case SpeedUnit.SI:
                    targetValue = pivotValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return System.Convert.ToInt32(targetValue);
        }


    }
}
