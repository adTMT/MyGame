using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class Coin
    {
        public Vector2 Position { get; private set; }
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        private Texture2D texture;

        public Coin(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.Yellow);
        }
    }
}
