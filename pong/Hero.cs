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
using pong.Levels;

namespace pong
{
    class Hero:IGameObject
    {
        Texture2D Herotexture;
        Texture2D Hitboxtexture;
        Animatie animatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private IInputReader inputReader;
        // Frame timer variabelen
        private float frameTimer;
        private float frameDuration; // Duur per frame, bepaalt de snelheid
        private float attackCooldownTimer = 0f;
        private float attackCooldown = 0.1f;
        public Rectangle Hitbox
        {
            get
            {
                // Return een rechthoek gebaseerd op de huidige positie en framegrootte
                return new Rectangle(
                    (int)positie.X + 6,
                    (int)positie.Y + 12,
                    animatie.CurrentFrame.SourceRectangle.Width -4,
                    animatie.CurrentFrame.SourceRectangle.Height -10
                );
            }
        }


        public Hero(Texture2D texture, IInputReader inputReader)
        {
            Herotexture = texture;
            this.inputReader = inputReader;
            animatie = new Animatie();
            positie = new Vector2(1, 1);
            snelheid = new Vector2(2, 2);
            frameTimer = 0f;
            frameDuration = 0.7f;

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
            //attack animaties
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(0,  64, 32, 32)));
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(32, 64, 32, 32)));
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(64, 64, 32, 32)));
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(96, 64, 32, 32)));
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(128,64, 32, 32)));
            animatie.AddFrame(ActionType.Attack, new AnimationFrames(new Rectangle(160,64, 32, 32)));
        }

        public void Update(GameTime gameTime, Level1 level, List<Enemy> enemies)
        {
            // Verlaag de cooldown timer
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            Move(level);

            // Verhoog de frame timer met de verstreken tijd sinds de laatste update
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Specifieke frameDuration voor de aanval animatie (trager)
            float currentFrameDuration = frameDuration;
            if (animatie.currentAction == ActionType.Attack)
            {
                currentFrameDuration = 0.1f; // Verhoog de frame duur voor de aanval
            }

            // Als de frame timer de ingestelde frame duur overschrijdt, ga dan naar de volgende frame
            if (frameTimer >= currentFrameDuration)
            {
                animatie.Update(); // Update de animatie
                frameTimer = 0f; // Reset de timer
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && attackCooldownTimer <= 0)
            {
                Attack(enemies); // Voer de aanval uit
                animatie.SetAction(ActionType.Attack);
                animatie.Update();// Zet animatie naar "Attack"
                attackCooldownTimer = attackCooldown;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D hitboxTexture)
        {
            
            spriteBatch.Draw(Herotexture, positie, animatie.CurrentFrame.SourceRectangle, Color.White);
            spriteBatch.Draw(hitboxTexture, Hitbox, Color.Red * 0.5f); // Transparante rode hitbox
        }
        public void Move(Level1 level)
        {
            var direction = inputReader.ReadInput();

            // Controleer of er input is
            if (direction != Vector2.Zero)
            {
                // Bereken toekomstige positie en toekomstige hitbox
                Vector2 toekomstigePositie = positie + direction * snelheid;
                Rectangle toekomstigeHitbox = new Rectangle(
                    (int)toekomstigePositie.X + 6,
                    (int)toekomstigePositie.Y + 12,
                    animatie.CurrentFrame.SourceRectangle.Width - 4,
                    animatie.CurrentFrame.SourceRectangle.Height - 10
                );

                // Controleer of de toekomstige positie binnen het scherm ligt en geen botsing heeft
                bool binnenScherm = toekomstigePositie.X >= 0 &&
                                    toekomstigePositie.X <= 800 - animatie.CurrentFrame.SourceRectangle.Width &&
                                    toekomstigePositie.Y >= 0 &&
                                    toekomstigePositie.Y <= 480 - animatie.CurrentFrame.SourceRectangle.Height;

                if (binnenScherm && !level.IsCollidingWithWall(toekomstigeHitbox))
                {
                    // Update positie en zet animatie op "Walk"
                    positie = toekomstigePositie;
                    animatie.SetAction(ActionType.Walk);
                }
                else
                {
                    // Zet animatie op "Idle" als beweging niet mogelijk is
                    animatie.SetAction(ActionType.Idle);
                }
            }
            else
            {
                // Geen input: zet animatie op "Idle"
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
        public bool CheckCollision(Rectangle otherObject)
        {
            return Hitbox.Intersects(otherObject);
        }
        public void Attack(List<Enemy> enemies)
        {
            // Bereken een hitbox voor de aanval
            Rectangle attackBounds = new Rectangle(
                (int)positie.X - 10, // De x-positie van de aanval
                (int)positie.Y - 10, // De y-positie van de aanval
                50,                  // Breedte van de aanval
                50                   // Hoogte van de aanval
            );

            // Controleer of een vijand geraakt wordt
            foreach (var enemy in enemies.ToList())
            {
                if (attackBounds.Intersects(enemy.Bounds))
                {
                    enemy.TakeDamage(10); // Reken schade toe
                }
            }

        }
    }
}
