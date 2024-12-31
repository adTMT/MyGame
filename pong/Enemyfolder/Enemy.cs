using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pong.Animations;
using pong.Interfaces;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace pong.Enemyfolder
{
    internal class Enemy : IHealth, IEnemy
    {
        public Vector2 Positie { get; set; }
        public int Health { get; set; }
        public Rectangle Bounds => new Rectangle((int)Positie.X, (int)Positie.Y, 32, 32); // Hitbox
        private Color color;
        public event Action<Enemy> OnDeath;
        //volg hero variabelen
        private float followRange = 150f;
        private float speed;
        private Vector2 snelheid = new Vector2(0, 1);
        private Vector2 snelheidx = new Vector2(1, 0);
        // Animatie variabelen
        private Animatie animatie;
        private float frameTimer;
        private float frameDuration = 0.2f;
        private float scale;
        private int AiType;

        public Enemy(Texture2D spritesheet, Vector2 positie, Color color, float schaal = 2f, float speed = 0.5f, int health = 100, int AiType = 0)
        {
            Positie = positie;
            this.color = color;
            Health = health;
            animatie = new Animatie();
            frameTimer = 0f;
            scale = schaal;
            this.speed = speed;
            this.AiType = AiType;
            //idle animaties
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(0, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(32, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(64, 0, 32, 32)));
            animatie.AddFrame(ActionType.Idle, new AnimationFrames(new Rectangle(96, 0, 32, 32)));

            animatie.SetAction(ActionType.Idle);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine("Damage given");

            if (Health <= 0)
            {
                Die();
            }
        }
        public void Follow(Vector2 heroPosition)
        {
            if (IsFollowing(heroPosition))
            {
                // Bereken richting naar de held
                Vector2 direction = Vector2.Normalize(heroPosition - Positie);
                Positie += direction * speed; // Beweeg richting de held
            }
        }
        private bool IsFollowing(Vector2 heroPosition)
        {
            float distance = Vector2.Distance(Positie, heroPosition);
            if (distance <= followRange)
                return true;
            else
                return false;
        }
        private void Die()
        {
            // Logica voor als de vijand sterft
            Console.WriteLine("Enemy defeated!");
            OnDeath?.Invoke(this);
        }

        private void Attack(Hero hero)
        {
            // Bereken een hitbox voor de aanval
            Rectangle attackBounds = new Rectangle(
                (int)Positie.X + 10, // De x-positie van de aanval
                (int)Positie.Y + 10, // De y-positie van de aanval
                30,                  // Breedte van de aanval
                30                   // Hoogte van de aanval
            );

            // Controleer of een vijand geraakt wordt
            if (attackBounds.Intersects(hero.Hitbox))
            {
                hero.TakeDamage(1);
            }

        }
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(
                            texture,
                            Positie,
                            animatie.CurrentFrame.SourceRectangle,
                            color,
                            0f,
                            Vector2.Zero,
                            scale,
                            SpriteEffects.None,
                            0f
                        );
        }

        public void Update(GameTime gameTime, Level level, Hero hero)
        {
            HandleAttack(hero);
            HandleAnimations(gameTime);
            HandleMovement(hero.positie);
        }
        private void HandleMovement(Vector2 heroPosition)
        {
            if (AiType == 1)
            {
                if (IsFollowing(heroPosition))
                {
                    Follow(heroPosition);
                }
                else
                {
                    Positie += snelheid;
                    if (Positie.Y > 480 - 120 || Positie.Y < 0)
                    {
                        snelheid.Y *= -1;
                    }
                }
            }
            else if (AiType == 2)
            {
                if (IsFollowing(heroPosition))
                {
                    Follow(heroPosition);
                }
                else
                {
                    Positie += snelheidx;
                    if (Positie.X > 800 - 64 || Positie.X < 0)
                    {
                        snelheidx.X *= -1;
                    }

                }
            }
            else
                Follow(heroPosition);
        }
        private void HandleAttack(Hero hero)
        {
            Attack(hero);
        }
        private void HandleAnimations(GameTime gameTime)
        {
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer >= frameDuration)
            {
                animatie.Update();
                frameTimer = 0f;
            }
        }
        public void Update(GameTime gameTime, Level level, List<Enemy> enemies)
        {
            throw new NotImplementedException();
        }
    }
}
