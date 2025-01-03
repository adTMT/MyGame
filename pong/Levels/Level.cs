﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pong.Enemyfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Levels
{
    internal class Level
    {
        private int[,] layout;                       // 2D-array voor de level-layout
        private List<Tile> tiles;             // Lijst met alle tiles
        private Texture2D tilesetTexture;     // De tileset-afbeelding
        private int tileSize;                 // Grootte van één tile (bijvoorbeeld 32x32)
        private Dictionary<int, Rectangle> tileMapping;  // Mapping van tile-ID naar tileset-coördinaten
        private int[,] originalLayout; //originele layout


        public Level(Texture2D tilesetTexture, int tileSize, Dictionary<int, Rectangle> tileMapping)
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
            originalLayout = layout;
        }
        public void Reset()
        {
            // Herstel de layout naar de originele staat
            layout = originalLayout;
        }
        public void Update(GameTime gameTime, Level level, Hero hero, EnemyManager enemyManager)
        {
            enemyManager.UpdateEnemies(gameTime, level, hero);
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
        public bool IsCollidingWithWall(Rectangle heroBounds)
        {
            for (int y = 0; y < layout.GetLength(0); y++)
            {
                for (int x = 0; x < layout.GetLength(1); x++)
                {
                    int tileId = layout[y, x];
                    if (tileId == 1) // 1 staat voor muur in je tileMapping
                    {
                        Rectangle tileBounds = new Rectangle(
                            x * tileSize, // Berekent de tegelpositie op het scherm
                            y * tileSize,
                            tileSize,
                            tileSize
                        );

                        if (tileBounds.Intersects(heroBounds))
                        {
                            return true; // Er is een botsing
                        }
                    }
                }
            }

            return false; // Geen botsing
        }
    }
}
