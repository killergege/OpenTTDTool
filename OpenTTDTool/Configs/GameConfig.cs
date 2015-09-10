using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    class GameConfig
    {
        #region Singleton
        private static GameConfig instance = null;
        public static GameConfig Instance
        {
            get { return instance = instance ?? new GameConfig(); }
        }
        private GameConfig() { }
        #endregion

        public int Multiplier { get; set; }
    }
}

