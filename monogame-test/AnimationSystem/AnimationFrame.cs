using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.AnimationSystem
{
    public class AnimationFrame
    {
        public SpriteFrame SpriteFrame { get; set; }
        public TimeSpan Duration { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
    }
}
