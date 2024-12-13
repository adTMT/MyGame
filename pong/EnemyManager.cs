using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using pong.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class EnemyManager
    {
        private EnemyFactory enemyFactory;
        public List<Enemy> enemies;
        private EnemyDeathHandler deathHandler;

        public EnemyManager(Texture2D enemyTexture)
        {
            enemyFactory = new EnemyFactory(enemyTexture);
            enemies = new List<Enemy>();
            deathHandler = new EnemyDeathHandler(enemies);
        }

        public void AddEnemy(Vector2 position, Color color, float scale = 2f, float speed = 0.5f, int health = 100)
        {
            var enemy = enemyFactory.CreateEnemy(position, color, scale, speed, health);
            enemy.OnDeath += deathHandler.HandleDeath; // Gebruik de handler voor doden
            enemies.Add(enemy);
        }

        public void UpdateEnemies(GameTime gameTime, Level1 level, Hero hero)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime, level, hero);
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch, enemyFactory.enemyTexture);
            }
        }
    }
}
