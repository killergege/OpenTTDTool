using OpenTTDTool.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    public class Train : Vehicle
    {
        public int TrackType { get; set; }
        public int Speed { get; set; }
        public int Power { get; set; }
        public int RunningCost { get; set; }
        public int RunningCostFactor { get; set; }
        public int TractionType { get; set; }
        public int TractiveEffort { get; set; }
        public int AirDrag { get; set; }
        public int SizeReduction { get; set; }
        public int CargoCapacity { get; set; }
        public int LongDateOfIntroduction { get; set; }
        public int Weight { get; set; }
        public int CargoType { get; set; }
        public int ReffitableCargo { get; set; }

        public string ReffitableCargoLabels
        {
            get
            {
                return string.Join(", ", getRefitListString());
            }
        }

        public int FullWeight
        {
            //TODO : do not account for multiplier if passenger
            get
            {
                return Weight + CargoCapacity * GameConfig.getInstance().Multiplier;
            }
        }


        public int LocalizedSpeed
        {
            get
            {
                double realSpeed = Speed / 1.6;
                return LocalizationConfig.getInstance().Convert(Convert.ToInt32(realSpeed));
            }
        }

        private List<Cargo.CargoTypes> getRefitList()
        {
            return Cargo.readCargoTypes((Cargo.CargoTypes)ReffitableCargo);
        }

        private List<string> getRefitListString()
        {
            List<string> list = new List<string>();
            getRefitList().ForEach(p => list.Add(p.GetDescription()));
            return list;
        }

        static Train()
        {
            PropertyDefinition.Add(0x05, new PropertyInfo() { PropertyName = nameof(TrackType) });
            PropertyDefinition.Add(0x09, new PropertyInfo() { Length = 2, PropertyName = nameof(Speed) });
            PropertyDefinition.Add(0x0B, new PropertyInfo() { Length = 2, PropertyName = nameof(Power) });
            PropertyDefinition.Add(0x0E, new PropertyInfo() { Length = 4, PropertyName = nameof(RunningCost) });
            PropertyDefinition.Add(0x0D, new PropertyInfo() { PropertyName = nameof(RunningCostFactor) });
            PropertyDefinition.Add(0x14, new PropertyInfo() { Length = 1, PropertyName = nameof(CargoCapacity) });
            PropertyDefinition.Add(0x15, new PropertyInfo() { PropertyName = nameof(CargoType) });
            PropertyDefinition.Add(0x16, new PropertyInfo() { PropertyName = nameof(Weight) });
            PropertyDefinition.Add(0x19, new PropertyInfo() { PropertyName = nameof(TractionType) });
            PropertyDefinition.Add(0x1F, new PropertyInfo() { PropertyName = nameof(TractiveEffort) });
            PropertyDefinition.Add(0x20, new PropertyInfo() { PropertyName = nameof(AirDrag) });
            PropertyDefinition.Add(0x21, new PropertyInfo() { PropertyName = nameof(SizeReduction) });
            PropertyDefinition.Add(0x28, new PropertyInfo() { Length = 2 , PropertyName = nameof(ReffitableCargo) });
            PropertyDefinition.Add(0x2A, new PropertyInfo() { Length = 4, PropertyName = nameof(LongDateOfIntroduction) });

            PropertyDefinition.Add(0x08, new PropertyInfo());
            PropertyDefinition.Add(0x12, new PropertyInfo());
            PropertyDefinition.Add(0x13, new PropertyInfo());
            PropertyDefinition.Add(0x17, new PropertyInfo());
            PropertyDefinition.Add(0x18, new PropertyInfo());
            PropertyDefinition.Add(0x1A, new PropertyInfo());
            PropertyDefinition.Add(0x1B, new PropertyInfo() { Length = 2 });
            PropertyDefinition.Add(0x1C, new PropertyInfo());
            PropertyDefinition.Add(0x1D, new PropertyInfo() { Length = 4 });
            PropertyDefinition.Add(0x1E, new PropertyInfo());
            PropertyDefinition.Add(0x22, new PropertyInfo());
            PropertyDefinition.Add(0x23, new PropertyInfo());
            PropertyDefinition.Add(0x24, new PropertyInfo());
            PropertyDefinition.Add(0x25, new PropertyInfo());
            PropertyDefinition.Add(0x26, new PropertyInfo());
            PropertyDefinition.Add(0x27, new PropertyInfo());
            PropertyDefinition.Add(0x29, new PropertyInfo() { Length = 2 });
            PropertyDefinition.Add(0x2B, new PropertyInfo() { Length = 2 });
            PropertyDefinition.Add(0x2C, new PropertyInfo() { Length = Constants.PROPERTY_SPECIAL_LENGTH_VALUE });
            PropertyDefinition.Add(0x2D, new PropertyInfo() { Length = Constants.PROPERTY_SPECIAL_LENGTH_VALUE });
        }


    }
}
