using Microsoft.Xna.Framework.Graphics;
using pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using pong.Animations;
using Microsoft.Xna.Framework.Input;
using pong.Input;

namespace pong
{
    class Hero:IGameObject
    {
        Texture2D Herotexture;
        Animatie animatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private IInputReader inputReader;



        public Hero(Texture2D texture, IInputReader inputReader)
        {
            Herotexture = texture;
            this.inputReader = inputReader;
            animatie = new Animatie();
            positie = new Vector2(1, 1);
            snelheid = new Vector2(2, 2);
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
            //MoveWithMouse();
            Move();
            animatie.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Herotexture, positie, animatie.CurrentFrame.SourceRectangle, Color.White);
        }
        public void Move()
        {
            var direction = inputReader.ReadInput();
            direction *= snelheid;
            positie += direction;
        }
        public void MoveWithMouse()
        {
            MouseState state = Mouse.GetState();
            Vector2 mouseVector = new Vector2(state.X, state.Y);

            var afstandofrichting = mouseVector - positie;
            afstandofrichting.Normalize();
            var afteleggenafstand = afstandofrichting * snelheid;

            positie += afteleggenafstand;
        }
    }
}
