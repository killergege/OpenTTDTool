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
                return string.Join(", ", RefitListString);
            }
        }

        public int FullWeight
        {
            //TODO : do not account for multiplier if passenger
            get
            {
                return Weight + CargoCapacity * GameConfig.Instance.Multiplier;
            }
        }


        public int LocalizedSpeed
        {
            get
            {
                double realSpeed = Speed / 1.6;
                return LocalizationConfig.Instance.Convert(Convert.ToInt32(realSpeed));
            }
        }

        private List<Cargo.CargoTypes> RefitList
        {
            get
            {
                return Cargo.ReadCargoTypes((Cargo.CargoTypes)ReffitableCargo);
            }
        }

        private List<string> RefitListString
        {
            get
            {
                return RefitList.Select(p => p.GetDescription()).ToList();
            }
        }

        static Train()
        {
            PropertyDefinition.Add(new PropertyInfoId(0x05, Features.Trains), new PropertyInfo() { PropertyName = nameof(TrackType) });
            PropertyDefinition.Add(new PropertyInfoId(0x09, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word, PropertyName = nameof(Speed) });
            PropertyDefinition.Add(new PropertyInfoId(0x0B, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word, PropertyName = nameof(Power) });
            PropertyDefinition.Add(new PropertyInfoId(0x0E, Features.Trains), new PropertyInfo() { Length = FieldSizes.DoubleWord, PropertyName = nameof(RunningCost) });
            PropertyDefinition.Add(new PropertyInfoId(0x0D, Features.Trains), new PropertyInfo() { PropertyName = nameof(RunningCostFactor) });
            PropertyDefinition.Add(new PropertyInfoId(0x14, Features.Trains), new PropertyInfo() { Length = FieldSizes.Byte, PropertyName = nameof(CargoCapacity) });
            PropertyDefinition.Add(new PropertyInfoId(0x15, Features.Trains), new PropertyInfo() { PropertyName = nameof(CargoType) });
            PropertyDefinition.Add(new PropertyInfoId(0x16, Features.Trains), new PropertyInfo() { PropertyName = nameof(Weight) });
            PropertyDefinition.Add(new PropertyInfoId(0x19, Features.Trains), new PropertyInfo() { PropertyName = nameof(TractionType) });
            PropertyDefinition.Add(new PropertyInfoId(0x1F, Features.Trains), new PropertyInfo() { PropertyName = nameof(TractiveEffort) });
            PropertyDefinition.Add(new PropertyInfoId(0x20, Features.Trains), new PropertyInfo() { PropertyName = nameof(AirDrag) });
            PropertyDefinition.Add(new PropertyInfoId(0x21, Features.Trains), new PropertyInfo() { PropertyName = nameof(SizeReduction) });
            PropertyDefinition.Add(new PropertyInfoId(0x28, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word, PropertyName = nameof(ReffitableCargo) });
            PropertyDefinition.Add(new PropertyInfoId(0x2A, Features.Trains), new PropertyInfo() { Length = FieldSizes.DoubleWord, PropertyName = nameof(LongDateOfIntroduction) });

            PropertyDefinition.Add(new PropertyInfoId(0x08, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x12, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x13, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x17, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x18, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x1A, Features.Trains), new PropertyInfo()); // !!!!! Since OpenTTD r13831 this is an extended byte
            PropertyDefinition.Add(new PropertyInfoId(0x1B, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word });
            PropertyDefinition.Add(new PropertyInfoId(0x1C, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x1D, Features.Trains), new PropertyInfo() { Length = FieldSizes.DoubleWord });
            PropertyDefinition.Add(new PropertyInfoId(0x1E, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x22, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x23, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x24, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x25, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x26, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x27, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x29, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word });
            PropertyDefinition.Add(new PropertyInfoId(0x2B, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word });
            PropertyDefinition.Add(new PropertyInfoId(0x2C, Features.Trains), new PropertyInfo() { Length = FieldSizes.MultipleBytes });
            PropertyDefinition.Add(new PropertyInfoId(0x2D, Features.Trains), new PropertyInfo() { Length = FieldSizes.MultipleBytes});
        }
    }
}
