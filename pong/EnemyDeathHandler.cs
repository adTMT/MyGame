using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    internal class EnemyDeathHandler
    {
        private List<Enemy> enemies;

        public EnemyDeathHandler(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        public void HandleDeath(Enemy deadEnemy)
        {
            enemies.Remove(deadEnemy);
        }
    }
}
