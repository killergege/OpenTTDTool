using OpenTTDTool.Configs;
using OpenTTDTool.Entities.SupportEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities.GameEntities
{
    public class Train : Vehicle
    {
        #region Raw properties
        //TODO : Gerer un tag pour voir les propriétés localisées
        public int TrackType { get; set; }
        public int Speed { get; set; }
        public int Power { get; set; }
        public int RunningCost { get; set; }
        public int RunningCostFactor { get; set; }
        public int TractionType { get; set; }
        public int TractiveEffortCoefficient { get; set; }
        public int AirDrag { get; set; }
        public int SizeReduction { get; set; }
        public int CargoCapacity { get; set; }
        public int LongDateOfIntroduction { get; set; }
        public int Weight { get; set; }
        public int CargoType { get; set; }
        public int ReffitableCargo { get; set; }
        public int CostFactor { get; set; }
        public int OrderKey { get; set; }
        #endregion

        public Train(int Id) : base(Id)
        {
            //Default values
            TractionType = 0;
            CargoCapacity = 0;
            Power = 0;
            VehicleLife = 0;
            TractiveEffortCoefficient = 0x4C;
        }

        public Train():base()
        {

        }

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

        public int TractiveEffort
        {
            get
            {
                double realTractiveEffortCoefficient = (double)TractiveEffortCoefficient / 0xFF;
                double realTractiveEffort = Weight * Constants.GRAVITY * realTractiveEffortCoefficient;
                //TODO : Arrondi incorrect par rapport à la valeur ingame *pan je vais me pendre*
                return Convert.ToInt32(Math.Round(realTractiveEffort, 0));
            }
        }

        public int LocalizedSpeed
        {
            get
            {
                double realSpeed = Math.Round(Speed / 1.6, 0);
                //TODO : Rounding in not the same as ingame
                return LocalizationConfig.Instance.Convert(Convert.ToInt32(realSpeed));
            }
        }

        public int Cost
        {
            get
            {
                double calculatedCost = Constants.COST_FACTOR_BASE * CostFactor;

                //Don't know why (not documented) but it seems we need to divide by 100 for wagon and 2 for trains
                //TODO : CHeck if this is always true
                int divider;
                if (IsWagon)
                {
                    divider = 100;
                }
                else
                {
                    divider = 2;
                }
                return Convert.ToInt32(Math.Truncate(calculatedCost / divider));
            }
        }

        public bool IsWagon
        {
            get
            {
                //TODO : Check that for powered wagon the power raw data is really 0 (powered rail should work with another property)
                return Power == 0;
            }
        }

        private List<Cargo.CargoCategory> RefitList
        {
            get
            {
                return Cargo.ReadCargoTypes((Cargo.CargoCategory)ReffitableCargo);
            }
        }

        private List<string> RefitListString
        {
            get
            {
                return RefitList.Select(p => p.GetDescription()).ToList();
            }
        }

        public override int GetOrderKey()
        {
            //TODO : Determine how the game sort this
            //We want the wagon after so we add the max id to wagon id
            if (IsWagon)
                return 65535 + OrderKey;
            else
                return OrderKey;
        }

        public override string GameDisplay(string LinePrefix = "")
        {
            //Cost : XXX Weight : XXX
            //Speed : XXX Power : XXX
            //Max. Tractive Effort : XXX
            //Running Cost : xxx
            //Capacity : XXX
            //Designed : XXX Life : XXX
            //Max. Reliability : XXX
            StringBuilder sb = new StringBuilder();

            sb.Append(LinePrefix);

            sb.Append("Cost : ");
            sb.Append("£"); //TODO : Make CurrencyUnit a config where raw is £ (dollar)
            sb.Append(Cost);

            if (IsWagon)
            {
                sb.Append(Environment.NewLine);
                sb.Append(LinePrefix);
                sb.Append("Weight : ");
            }
            else
            {
                sb.Append(" Weight : ");
            }
            sb.Append(Weight);
            //TODO : En fait l'unité dépend de ce qui est transporté (solide ou liquide, etc.)
            sb.Append(" t"); //TODO : Make WeightUnit a config where raw is t (tonne)
            if (IsWagon)
            {
                sb.Append(" (");
                //TODO : Le calcul semble complètement à coté de la plaque
                sb.Append(FullWeight);
                //TODO : En fait l'unité dépend de ce qui est transporté (solide ou liquide, etc.)
                sb.Append(" t)"); //TODO : Make WeightUnit a config where raw is t (tonne)
            }
            sb.Append(Environment.NewLine);
            sb.Append(LinePrefix);

            sb.Append("Speed : ");
            sb.Append(LocalizedSpeed);
            sb.Append(" km/h");

            if (!IsWagon)
            {
                sb.Append(" Power : ");
                sb.Append(Power);
                sb.Append(" hp"); //TODO : Make PowerUnit a config where raw is hp (imperial horsepower)
            }

            sb.Append(Environment.NewLine);
            sb.Append(LinePrefix);

            if (!IsWagon)
            {
                sb.Append("Max. Tractive Effort : ");
                sb.Append(TractiveEffort);
                sb.Append(" kN"); //TODO : Make TractiveEffortUnit a config where raw is kN (SI kile Newton)

                sb.Append(Environment.NewLine);
                sb.Append(LinePrefix);
            }

            sb.Append("Capacity : ");
            if (CargoCapacity == 0)
            {
                sb.Append("N/A");
            }
            else
            {
                sb.Append(CargoCapacity);
                //TODO : En fait l'unité dépend de ce qui est transporté (solide ou liquide, etc.)
                sb.Append(" t"); //TODO : Make WeightUnit a config where raw is t (tonne)
            }


            sb.Append(Environment.NewLine);
            sb.Append(LinePrefix);

            sb.Append("Designed : ");
            //TODO : Correct base year to display proper year
            //TODO : Handle default value (do not display if default)
            sb.Append(ModelLife);

            if (VehicleLife != 0x00 && VehicleLife != 0xFF)
            {
                sb.Append(" Life : ");
                sb.Append(VehicleLife);
                sb.Append(" years");
            }

            sb.Append(Environment.NewLine);
            sb.Append(LinePrefix);

            sb.Append("Max. Reliability : ");
            sb.Append("unknown");

            return sb.ToString();
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
            PropertyDefinition.Add(new PropertyInfoId(0x17, Features.Trains), new PropertyInfo() { Length = FieldSizes.Byte, PropertyName = nameof(CostFactor) });
            PropertyDefinition.Add(new PropertyInfoId(0x19, Features.Trains), new PropertyInfo() { PropertyName = nameof(TractionType) });
            PropertyDefinition.Add(new PropertyInfoId(0x1A, Features.Trains), new PropertyInfo() { Length = FieldSizes.ExtendedByte, PropertyName = nameof(OrderKey) }); // !!!!! Since OpenTTD r13831 this is an extended byte
            PropertyDefinition.Add(new PropertyInfoId(0x1F, Features.Trains), new PropertyInfo() { PropertyName = nameof(TractiveEffortCoefficient) });
            PropertyDefinition.Add(new PropertyInfoId(0x20, Features.Trains), new PropertyInfo() { PropertyName = nameof(AirDrag) });
            PropertyDefinition.Add(new PropertyInfoId(0x21, Features.Trains), new PropertyInfo() { PropertyName = nameof(SizeReduction) });
            PropertyDefinition.Add(new PropertyInfoId(0x28, Features.Trains), new PropertyInfo() { Length = FieldSizes.Word, PropertyName = nameof(ReffitableCargo) });
            PropertyDefinition.Add(new PropertyInfoId(0x2A, Features.Trains), new PropertyInfo() { Length = FieldSizes.DoubleWord, PropertyName = nameof(LongDateOfIntroduction) });

            PropertyDefinition.Add(new PropertyInfoId(0x08, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x12, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x13, Features.Trains), new PropertyInfo());
            PropertyDefinition.Add(new PropertyInfoId(0x18, Features.Trains), new PropertyInfo());
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
            PropertyDefinition.Add(new PropertyInfoId(0x2D, Features.Trains), new PropertyInfo() { Length = FieldSizes.MultipleBytes });
        }
    }
}
