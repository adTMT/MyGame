using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Levels
{
    internal class LevelManager
    {
        private Dictionary<LevelSelect, Level> levels;
        private LevelSelect currentLevel;

        public LevelManager()
        {
            levels = new Dictionary<LevelSelect, Level>();
        }

        public void AddLevel(LevelSelect levelId, Level level)
        {
            levels[levelId] = level;
        }

        public void SetCurrentLevel(LevelSelect levelId)
        {
            if (levels.ContainsKey(levelId))
            {
                currentLevel = levelId;
            }
        }

        public Level GetCurrentLevel()
        {
            return levels.ContainsKey(currentLevel) ? levels[currentLevel] : null;
        }

        public void ResetCurrentLevel()
        {
            if (levels.ContainsKey(currentLevel))
            {
                levels[currentLevel].Reset();
            }
            else
            {
                Console.WriteLine("No level is set to reset.");
            }
        }
    }
}
