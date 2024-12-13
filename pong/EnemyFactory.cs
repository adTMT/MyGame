using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class EnemyFactory
    {
        public Texture2D enemyTexture;


        public EnemyFactory(Texture2D texture)
        {
            enemyTexture = texture;
        }

        public Enemy CreateEnemy(Vector2 position, Color color, float scale = 2f, float speed = 0.5f, int health = 100)
        {
            return new Enemy(enemyTexture, position, color, scale, speed, health);
        }
    }
}
