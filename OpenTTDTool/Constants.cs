using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    public enum Actions
    {
        Properties = 0x00,
        Labels = 0x04,
        Conditions = 0x07,
        Headers = 0x08,
        Conditions2 = 0x09
    }

    public enum Features
    {
        Trains = 0x00,
        RoadVehicles = 0x01,
        Ships = 0x02,
        Aircraft = 0x03,
        Stations = 0x04,
        Canals = 0x05,
        Bridges = 0x06,
        Houses = 0x07,
        GlobalSettings = 0x08,
        IndustryTiles = 0x09,
        Industries = 0x0A,
        Cargos = 0x0B,
        SoundEffects = 0x0C,
        Airports = 0x0D,
        Objects = 0x0F,
        Railtypes = 0x10,
        AirportTiles = 0x11,
        Original = 0x48
    }

    public class Constants
    {
        public const int DEFAULT_LANGUAGE = 0x7F;

        public const int INDEX_SPRITES = 1;
        public const int INDEX_ACTIONS = 3;
        public const int INDEX_FEATURES = 4;
        public const int INDEX_CLEANABLE = 5;
        public const int INDEX_LANGUAGE = 5;
        public const int INDEX_IDENTIFIER = 7;
        public const int INDEX_IDENTIFIER_AND_LABEL = 7;
        public const int INDEX_LABEL = 8;

        public const int PROPERTY_LABEL_CODE = -0x01;
        public const string PROPERTY_SPRITE_NONE = "*";
        public const int PROPERTY_SPECIAL_LENGTH_VALUE = -0x01;

        public const int DATA_TRAIN_INDEX_NB_VEHICLES = 6;
        public const int DATA_TRAIN_INDEX_PROPERTIES = 8;

        public const int CODE_PAGE_NFO = 437;
    }
}
