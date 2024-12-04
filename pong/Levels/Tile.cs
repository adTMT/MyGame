using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Levels
{
    internal class Tile
    {
        public Rectangle SourceRectangle { get; private set; } // Deel van de tileset
        public Vector2 Position { get; private set; }          // Wereldpositie van de tile
        public bool IsSolid { get; private set; }              // Is de tile solid (botsing)?

        public Tile(Rectangle sourceRectangle, Vector2 position, bool isSolid)
        {
            SourceRectangle = sourceRectangle;
            Position = position;
            IsSolid = isSolid;
        }
    }
}
