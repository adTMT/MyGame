using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Interfaces
{
    internal interface IAttackable
    {
        void Attack(List<Enemy> enemies);
    }
}
