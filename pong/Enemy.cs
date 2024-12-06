using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class Enemy
    {
        public Vector2 Positie { get; set; }
        public int Health { get; private set; }
        public Rectangle Bounds => new Rectangle((int)Positie.X, (int)Positie.Y, 32, 32); // Hitbox

        public Enemy(Vector2 positie)
        {
            Positie = positie;
            Health = 100;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Logica voor als de vijand sterft
             Console.WriteLine("Enemy defeated!");
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Positie, Color.Red);
        }
    }
}
