using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong.Animations
{
    public class Animatie
    {
        public AnimationFrames CurrentFrame { get; set; }

        private List<AnimationFrames> frames;
        private int counter;
        public Animatie()
        {
            frames = new List<AnimationFrames>();
        }
        public void AddFrame(AnimationFrames animationFrames)
        {
            frames.Add(animationFrames);
            CurrentFrame = frames[0];
        }
        public void Update()
        {
            CurrentFrame = frames[counter];
            counter++;
            if (counter >= frames.Count)
                counter = 0;
        }
    }
}
