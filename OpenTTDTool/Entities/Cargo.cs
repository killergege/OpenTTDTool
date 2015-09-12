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
        public enum CargoCategory
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

        //TODO: Faire une classe à part pour gerer les associations Category <=> Type ?
        //TODO : Ajouter le type d'unité associé
        public enum CargoType
        {
            [Description("Passengers")]
            Passengers = 0x00,
            [Description("Coal")]
            Coal = 0x01,
            [Description("Mail")]
            Mail = 0x02,
            [Description("Oil")]
            Oil = 0x03,
            [Description("Livestock")]
            Livestock = 0x04,
            [Description("Goods")]
            Goods = 0x05,
            [Description("Grain/Wheat/Maize")]
            Grain = 0x06,
            [Description("Wood")]
            Wood = 0x07,
            [Description("Iron Ore ")]
            IronOre = 0x08,
            [Description("Steel")]
            Steel = 0x09,
            [Description("Valuables/Gold/Diamonds ")]
            Valuables = 0x0A,
            [Description("Paper")]
            Paper = 0x0B,
            [Description("Food")]
            Food = 0x0C,
            [Description("Fruit")]
            Fruit = 0x0D,
            [Description("Copper Ore ")]
            CopperOre = 0x0E,
            [Description("Water")]
            Water = 0x0F,
            [Description("Rubber")]
            Rubber = 0x10,
            [Description("Sugar")]
            Sugar = 0x11,
            [Description("Toys")]
            Toys = 0x121,
            [Description("Batteries")]
            Batteries = 0x13,
            [Description("Candy (Sweets) ")]
            Candy = 0x14,
            [Description("Toffee")]
            Toffee = 0x15,
            [Description("Cola")]
            Cola = 0x16,
            [Description("Cotton Candy (Candyfloss)")]
            Candyfloss = 0x17,
            [Description("Bubbles")]
            Bubbles = 0x18,
            [Description("Plastic")]
            Plastic = 0x19,
            [Description("Fizzy Drinks ")]
            FizzyDrinks = 0x1A,
            [Description("Paper")]
            Paper2 = 0x1B
        }

        public static List<CargoCategory> ReadCargoTypes(CargoCategory bitmask)
        {
            var types = new List<CargoCategory>();

            foreach (CargoCategory type in Enum.GetValues(typeof(CargoCategory)))
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
