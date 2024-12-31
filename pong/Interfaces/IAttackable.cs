using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pong.Enemyfolder;

namespace pong.Interfaces
{
    internal interface IAttackable
    {
        void Attack(List<Enemy> enemies);
    }
}
