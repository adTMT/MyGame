using Microsoft.Xna.Framework.Graphics;
using pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace pong
{
    class Hero:IGameObject
    {
        Texture2D Herotexture;
        private Rectangle deelrectangle;
        private int schuifOp_X = 0;
        public Hero(Texture2D texture)
        {
            Herotexture = texture;
            deelrectangle = new Rectangle(schuifOp_X, 0, 32, 32);

        }

        public void Update()
        {
            schuifOp_X += 32;
            if (schuifOp_X > 256)
            {
                schuifOp_X = 0;
            }
            deelrectangle.X = schuifOp_X;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Herotexture, new Vector2(10,10), deelrectangle, Color.White);
        }
    }
}
