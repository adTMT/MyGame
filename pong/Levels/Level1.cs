using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Levels
{
    internal class Level1
    {
        private int[,] layout;                       // 2D-array voor de level-layout
        private List<Tile> tiles;             // Lijst met alle tiles
        private Texture2D tilesetTexture;     // De tileset-afbeelding
        private int tileSize;                 // Grootte van één tile (bijvoorbeeld 32x32)
        private Dictionary<int, Rectangle> tileMapping;  // Mapping van tile-ID naar tileset-coördinaten


        public Level1(Texture2D tilesetTexture, int tileSize, Dictionary<int, Rectangle> tileMapping)
        {
            this.tilesetTexture = tilesetTexture;
            this.tileSize = tileSize;
            tiles = new List<Tile>();
            this.tileMapping = tileMapping;
        }

        // Methode om de levelgegevens te laden
        public void LoadLevel(int[,] layout)
        {
            this.layout = layout;
        }

        // Tekenen van het level
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < layout.GetLength(0); y++)
            {
                for (int x = 0; x < layout.GetLength(1); x++)
                {
                    int tileId = layout[y, x];
                    if (tileMapping.ContainsKey(tileId))
                    {
                        Rectangle sourceRectangle = tileMapping[tileId];
                        Rectangle destinationRectangle = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);

                        spriteBatch.Draw(tilesetTexture, destinationRectangle, sourceRectangle, Color.White);
                    }
                }
            }
        }
    }
}
