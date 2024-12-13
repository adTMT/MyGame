using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Levels
{
    internal class LevelData
    {
        public int[,] Layout { get; set; }
        public Dictionary<int, Rectangle> TileMapping { get; set; }

        public LevelData(int[,] layout, Dictionary<int, Rectangle> tileMapping)
        {
            Layout = layout;
            TileMapping = tileMapping;
        }
    }
}
