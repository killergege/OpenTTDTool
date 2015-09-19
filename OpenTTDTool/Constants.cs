using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    /// <summary>
    /// Specs : http://newgrf-specs.tt-wiki.net/wiki/GRFActionsDetailed
    /// </summary>
    public enum FieldSizes
    {
        /// <summary>
        /// The value is given in decimal, not hexadecimal; the size is therefore irrelevant
        /// </summary>
        Decimal,
        /// <summary>
        /// A single byte
        /// </summary>
        Byte,
        /// <summary>
        /// An extended byte (either a byte, or a word value)
        /// Extended bytes work like this:
        ///     To specify a value of 0..FE, simply use that byte value
        ///     To specify a value of FF..FFFF, use a literal FF followed by the word value, e.g. for 320d (140 hex) use FF 40 01.
        /// </summary>
        ExtendedByte,
        /// <summary>
        /// A two-byte word, specified in little-endian byte order
        /// </summary>
        Word,
        /// <summary>
        /// A four-byte dword, again in little-endian byte order
        /// </summary>
        DoubleWord,
        /// <summary>
        /// Variable-length, zero terminated text string
        /// </summary>
        String,
        /// <summary>
        /// A variable length, which depends on one of the previous parameters
        /// </summary>
        Variable,
        /// <summary>
        /// B n*B
        /// </summary>
        MultipleBytes
    }

    public enum LineType
    {
        RealSprite,
        Sound,
        PseudoSprite
    }

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
        public const int LANGUAGE_UK = 0x03;

        public const int INDEX_ROW = 0;
        public const int INDEX_SPRITES = 1;
        public const int INDEX_SIZE = 2;
        public const int INDEX_ACTIONS = 3; //TODO : virer
        public const int INDEX_FEATURES = 4; //TODO : virer
        public const int INDEX_CLEANABLE = 5; //TODO : virer
        public const int INDEX_IDENTIFIER = 7; //TODO : virer
        public const int INDEX_EXTENDED_IDENTIFIER = 8; //TODO : virer

        public const int PROPERTY_LABEL_CODE = -0x01;
        public const string PROPERTY_SPRITE_NONE = "*";
        public const string PROPERTY_SPRITE_SOUND = "**";
        public const int PROPERTY_SPECIAL_LENGTH_VALUE = -0x01;

        public const int COST_FACTOR_BASE = 3125;

        public const double GRAVITY = 9.8;

        public const int MIN_NUMBER_OF_ELEMENTS = 5;

        //public const int DATA_TRAIN_INDEX_NB_VEHICLES = 6;
        //public const int DATA_TRAIN_INDEX_PROPERTIES = 8;

        public const int CODE_PAGE_NFO = 437;

        public static readonly int?[] SupportedLanguages = new int?[] { 0x7F, 0xFF, 0x01, 0x02, 0x03 };
        public static Dictionary<FieldSizes, int?> FieldLengths
        {
            get;
        } 
            = new Dictionary<FieldSizes, int?>()
                {
                    { FieldSizes.Byte, 1 },
                    { FieldSizes.Decimal, null },
                    { FieldSizes.DoubleWord, 4 },
                    { FieldSizes.ExtendedByte, null },
                    { FieldSizes.MultipleBytes, null },
                    { FieldSizes.String, 1 }, // TODO : vérifier que les chaines sont toujours lues correctement. Normalement le parseur met la chaine dans une seule et même "cellule"
                    { FieldSizes.Variable, null },
                    { FieldSizes.Word, 2 }
                };
    }
}
