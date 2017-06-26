using Microsoft.Xna.Framework;
using monogame_test.Core.AnimationSystem;
using monogame_test.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame_test.Core.AttackSystem
{
    public class Attack
    {
        public Animation Animation { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Vector2 BoundingBoxOrigin { get; set; }
        public Entity Owner { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// TODO: Look into this. Maybe represent it as an in-memory WAV, or something.
        /// Should attacks be responsible for loading their own sounds/animations?
        /// </summary>
        public string SoundPath { get; set; }
    }
}
