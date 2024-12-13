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
        private Texture2D _texture;
        private float _speed;
        private int _health;

        public EnemyFactory(Texture2D texture)
        {
            _texture = texture;
        }

        public Enemy CreateEnemy(Vector2 position, Color color, float speed = 0.5f, int health = 100)
        {
            return new Enemy(_texture, position, color, 2f, speed, health);
        }
    }
}
