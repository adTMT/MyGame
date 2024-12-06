using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Interfaces
{
    internal interface IGameObject
    {
        void Update(GameTime gameTime, Level1 level, List<Enemy> enemies);
        void Draw(SpriteBatch spriteBatch,Texture2D heroTexture);
    }
}
