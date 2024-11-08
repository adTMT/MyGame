using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pong.Animations
{
    public class AnimationFrames
    {
        public Rectangle SourceRectangle { get; set; }
        public AnimationFrames(Rectangle rectangle)
        {
            SourceRectangle = rectangle;
        }
    }
}
