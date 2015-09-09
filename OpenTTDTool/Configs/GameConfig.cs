using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    class GameConfig
    {
        private static GameConfig instance = null;

        public int Multiplier { get; set; }

        private GameConfig() { }

        public static GameConfig getInstance()
        {
            if (instance == null)
                instance = new GameConfig();
            return instance;
        }


    }
}
