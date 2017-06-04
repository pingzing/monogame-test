using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monogame_test.Entities;
using Microsoft.Xna.Framework;

namespace monogame_test.Components.Terra
{
    class TerraPhysicsComponent : IPhysicsComponent
    {
        public TerraPhysicsComponent()
        {

        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            entity.X += entity.XVelocity * delta;
            entity.Y += entity.YVelocity * delta;
        }
    }
}
