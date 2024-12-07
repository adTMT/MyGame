﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pong.Interfaces;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class Enemy: IGameObject
    {
        public Vector2 Positie { get; set; }
        public int Health { get; private set; }
        public Rectangle Bounds => new Rectangle((int)Positie.X, (int)Positie.Y, 32, 32); // Hitbox
        private Color color = Color.Green;
        public event Action<Enemy> OnDeath;
        private float followRange = 125f;
        private float speed = 0.5f;

        public Enemy(Vector2 positie)
        {
            Positie = positie;
            Health = 100;
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
            float distance = Vector2.Distance(Positie, heroPosition);

            if (distance <= followRange)
            {
                // Bereken richting naar de held
                Vector2 direction = Vector2.Normalize(heroPosition - Positie);
                Positie += direction * speed; // Beweeg richting de held
            }
        }

        private void Die()
        {
            // Logica voor als de vijand sterft
            Console.WriteLine("Enemy defeated!");
            OnDeath?.Invoke(this);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Positie, color);
        }

        public void Update(GameTime gameTime, Level1 level, List<Enemy> enemies)
        {
            throw new NotImplementedException();
        }
    }
}
