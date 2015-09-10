using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    static class Cargo
    {
        [Flags]
        public enum CargoTypes
        {
            [Description("Passengers")]
            Passengers = 1,
            [Description("Mail")]
            Mail = 2,
            [Description("Express")]
            Express = 4,
            [Description("Armored")]
            Armored = 8,
            [Description("Bulk")]
            Bulk = 16,
            [Description("PieceGoods")]
            PieceGoods = 32,
            [Description("Liquid")]
            Liquid = 64,
            [Description("Refrigerated")]
            Refrigerated = 128,
            [Description("Hazardous")]
            Hazardous = 256,
            [Description("Covered")]
            Covered = 512,
            [Description("Oversized")]
            Oversized = 1024,
            [Description("Powderized")]
            Powderized = 2048,
            [Description("NotPourable")]
            NotPourable = 4096,
            [Description("Reserved")]
            Reserved = 8192,
            [Description("Reserved2")]
            Reserved2 = 16384,
            [Description("Special")]
            Special = 32768
        }

        public static List<CargoTypes> ReadCargoTypes(CargoTypes bitmask)
        {
            var types = new List<CargoTypes>();

            foreach (CargoTypes type in Enum.GetValues(typeof(CargoTypes)))
            {
                if (bitmask.HasFlag(type))
                {
                    types.Add(type);
                }
            }

            return types;
        }
    }
}
