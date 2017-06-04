using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monogame_test.Entities;
using Microsoft.Xna.Framework;
using monogame_test.Maps;

namespace monogame_test.Components.Terra
{
    class TerraPhysicsComponent : IPhysicsComponent
    {
        public TerraPhysicsComponent()
        {            
        }

        public void Update(GameTime deltaTime, Entity entity, TestMap map)
        {            
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            Vector2 proposedNewPosition = new Vector2(entity.XVelocity * delta + entity.X, entity.YVelocity * delta + entity.Y);
            Rectangle proposedNewBoundingBox = new Rectangle((int)proposedNewPosition.X, (int)proposedNewPosition.Y, 14, 24);

            if (!map.GetIntersection(proposedNewBoundingBox).IsEmpty)
            {
                return;                
            }

            entity.X = proposedNewPosition.X;
            entity.Y = proposedNewPosition.Y;
            entity.BoundingBox = new Rectangle((int)entity.X, (int)entity.Y, 14, 24);
        }
    }
}
