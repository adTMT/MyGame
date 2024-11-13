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
        // Frame timer variabelen
        private float frameTimer;
        private float frameDuration; // Duur per frame, bepaalt de snelheid


        public Hero(Texture2D texture, IInputReader inputReader)
        {
            Herotexture = texture;
            this.inputReader = inputReader;
            animatie = new Animatie();
            positie = new Vector2(1, 1);
            snelheid = new Vector2(2, 2);
            frameTimer = 0f;
            frameDuration = 0.1f;
            //idle animaties
            animatie.AddFrame(ActionType.Idle,new AnimationFrames(new Rectangle(0, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(32, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(64, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(96, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(128, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(160, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(192, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(224, 0, 32, 32)));
            //walk animaties
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(0, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(32, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(64, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(96, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(128, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(160, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(192, 32, 32, 32)));
            animatie.AddFrame(ActionType.Walk, new AnimationFrames(new Rectangle(224, 32, 32, 32)));
        }

        public void Update(GameTime gameTime)
        {
            //MoveWithMouse();
            Move();
            // Verhoog de frame timer met de verstreken tijd sinds de laatste update
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Als de frame timer de ingestelde frame duur overschrijdt, ga dan naar de volgende frame
            if (frameTimer >= frameDuration)
            {
                animatie.Update(); // Update de animatie
                frameTimer = 0f; // Reset de timer
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Herotexture, positie, animatie.CurrentFrame.SourceRectangle, Color.White);
        }
        public void Move()
        {
            var direction = inputReader.ReadInput();
            if (direction != Vector2.Zero)
            {
                // Move the hero and set to "Walk" animation if there is input
                direction *= snelheid;
                positie += direction;
                animatie.SetAction(ActionType.Walk);
            }
            else
            {
                // Set to "Idle" animation if there is no input
                animatie.SetAction(ActionType.Idle);
            }
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
