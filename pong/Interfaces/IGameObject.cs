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
        Vector2 Positie { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        bool CheckCollision(Rectangle otherObject);
    }
}
