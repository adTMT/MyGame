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
using static System.Net.Mime.MediaTypeNames;

namespace pong
{
    class Hero : IHealth, IMovable, IAttackable
    {
        Texture2D Herotexture;
        Texture2D Hitboxtexture;
        Animatie animatie;
        public Vector2 positie;
        private Vector2 snelheid;
        private IInputReader inputReader;
        // Frame timer variabelen
        private float frameTimer;
        private float frameDuration; // Duur per frame, bepaalt de snelheid
        private float attackCooldownTimer = 0f;
        private float attackCooldown = 0.1f;
        public int Health { get; set; }
        public bool IsInvulnerable { get; private set; }
        private double invulnerabilityTimer;
        private double invulnerabilityDuration = 2.0; // 2 seconden onkwetsbaarheid
        private Color color = Color.White; // Normale kleur
        private float friction = 0.9f;
        private float maxSpeed = 2f;   // Maximale snelheid
        private float acceleration = 0.2f;
        private Vector2 currentSpeed;
        private float accelerationPower = 4f;
        public Rectangle Hitbox
        {
            get
            {
                // Return een rechthoek gebaseerd op de huidige positie en framegrootte
                return new Rectangle(
                    (int)positie.X + 6,
                    (int)positie.Y + 12,
                    animatie.CurrentFrame.SourceRectangle.Width - 14,
                    animatie.CurrentFrame.SourceRectangle.Height - 10
                );
            }
        }


        public Hero(Texture2D texture, IInputReader inputReader)
        {
            Herotexture = texture;
            this.inputReader = inputReader;
            animatie = new Animatie();
            positie = new Vector2(1, 1);
            snelheid = Vector2.Zero;
            frameTimer = 0f;
            frameDuration = 0.3f;
            Health = 5;


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

        public void Update(GameTime gameTime, Level level, List<Enemy> enemies)
        {
            HandleCooldown(gameTime);
            Move(level);
            HandleAnimations(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && attackCooldownTimer <= 0)
            {
                PerformAttack(enemies);
            }

            HandleInvulnerability(gameTime);


        }
        private void HandleCooldown(GameTime gameTime)
        {
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void HandleAnimations(GameTime gameTime)
        {
            // Verhoog de frame timer met de verstreken tijd
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Specifieke frameDuration voor de aanval animatie
            float currentFrameDuration = animatie.currentAction == ActionType.Attack ? 0.1f : frameDuration;

            if (frameTimer >= currentFrameDuration)
            {
                animatie.Update(); // Update de animatie
                frameTimer = 0f; // Reset de timer
            }
        }
        private void PerformAttack(List<Enemy> enemies)
        {
            Attack(enemies); // Voer de aanval uit
            animatie.SetAction(ActionType.Attack);
            animatie.Update(); // Zet animatie naar "Attack"
            attackCooldownTimer = attackCooldown;
        }
        private void HandleInvulnerability(GameTime gameTime)
        {
            if (IsInvulnerable)
            {
                invulnerabilityTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (invulnerabilityTimer >= invulnerabilityDuration)
                {
                    IsInvulnerable = false;
                    invulnerabilityTimer = 0;
                    color = Color.White; // Reset kleur
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsInvulnerable && (invulnerabilityTimer * 10 % 2) < 1)
            {
                // Sla renderen van hero over om hem te laten "knipperen"
                return;
            }
            spriteBatch.Draw(Herotexture, positie, animatie.CurrentFrame.SourceRectangle, color);
            //spriteBatch.Draw(hitboxTexture, Hitbox, Color.Red * 0.5f); // Transparante rode hitbox
        }
        private void DrawRectangle(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
        public void Move(Level level)
        {
            var direction = inputReader.ReadInput();

            // Controleer of er input is
            if (direction != Vector2.Zero)
            {
                // Versnel de beweging in de richting van de invoer
                currentSpeed += direction * acceleration;

                // Zorg ervoor dat de snelheid niet de maximale snelheid overschrijdt
                if (currentSpeed.Length() > maxSpeed)
                {
                    currentSpeed = Vector2.Normalize(currentSpeed) * maxSpeed;
                }

                // Bereken de toekomstige positie op basis van de huidige snelheid
                Vector2 toekomstigePositie = positie + currentSpeed;

                // Controleer of de toekomstige positie binnen het scherm ligt en geen botsing heeft
                Rectangle toekomstigeHitbox = new Rectangle(
                    (int)toekomstigePositie.X + 6,
                    (int)toekomstigePositie.Y + 12,
                    animatie.CurrentFrame.SourceRectangle.Width - 14,
                    animatie.CurrentFrame.SourceRectangle.Height - 10
                );

                bool binnenScherm = toekomstigePositie.X >= 0 &&
                                    toekomstigePositie.X <= 800 - animatie.CurrentFrame.SourceRectangle.Width &&
                                    toekomstigePositie.Y >= 0 &&
                                    toekomstigePositie.Y <= 480 - animatie.CurrentFrame.SourceRectangle.Height;

                if (binnenScherm && !level.IsCollidingWithWall(toekomstigeHitbox))
                {
                    // Update de positie van de hero en zet de animatie naar "Walk"
                    positie = toekomstigePositie;
                    animatie.SetAction(ActionType.Walk);
                }
                else
                {
                    currentSpeed = Vector2.Zero;

                    // Zet animatie naar "Idle" als er geen beweging is
                    animatie.SetAction(ActionType.Idle);
                }
            }
            else
            {
                animatie.SetAction(ActionType.Idle);
            }
            
        }
        private Vector2 BerekenToekomstigePositie(Vector2 huidigePositie, Vector2 direction, float snelheid)
        {
            return huidigePositie + direction * snelheid;
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
                (int)positie.X + 8, // De x-positie van de aanval
                (int)positie.Y + 16, // De y-positie van de aanval
                20,                  // Breedte van de aanval
                15                  // Hoogte van de aanval
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
        public void TakeDamage(int damage)
        {
            if (!IsInvulnerable)
            {
                Health -= damage;
                Console.WriteLine("Damage given");
                IsInvulnerable = true;
                color = Color.Red;
                if (Health <= 0)
                {
                    Die();
                }
            }
        }
        private void Die()
        {
            Health = 0;
        }
    }
}
