using Microsoft.Xna.Framework.Graphics;
using pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using pong.Animations;

namespace pong
{
    class Hero:IGameObject
    {
        Texture2D Herotexture;
        Animatie animatie;
        
        public Hero(Texture2D texture)
        {
            Herotexture = texture;
            animatie = new Animatie();
            animatie.AddFrame(new AnimationFrames(new Rectangle(0, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(32, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(64, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(96, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(128, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(160, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(192, 0, 32, 32)));
            animatie.AddFrame(new AnimationFrames(new Rectangle(224, 0, 32, 32)));
        }

        public void Update()
        {
            animatie.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Herotexture, new Vector2(10,10), animatie.CurrentFrame.SourceRectangle, Color.White);
        }
    }
}
