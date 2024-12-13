using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Interfaces
{
    internal interface IEnemy
    {
        void Update(GameTime gameTime, Level1 level, Hero hero);
        void Draw(SpriteBatch spriteBatch, Texture2D texture);
    }
}
