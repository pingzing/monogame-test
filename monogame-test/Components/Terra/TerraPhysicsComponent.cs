using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monogame_test.Entities;
using Microsoft.Xna.Framework;
using monogame_test.Maps;
using monogame_test.RenderHelpers;

namespace monogame_test.Components.Terra
{
    class TerraPhysicsComponent : IPhysicsComponent
    {
        private Point DefaultBoundingBoxSize = new Point(12, 10);

        public TerraPhysicsComponent()
        {            

        }

        public void Update(GameTime deltaTime, Entity entity, TestMap map)
        {       
            //Initialize bounding box
            if (entity.BoundingBox == RectangleF.Empty)
            {
                entity.BoundingBox = new RectangleF(
                    entity.Position.X, 
                    entity.Position.Y, 
                    (DefaultBoundingBoxSize.X * entity.Scale), 
                    (DefaultBoundingBoxSize.Y * entity.Scale));
            }
            
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            Vector2 proposedNewPosition = new Vector2(entity.Velocity.X * delta + entity.Position.X, entity.Velocity.Y * delta + entity.Position.Y);
            RectangleF proposedNewBoundingBox = new RectangleF(
                proposedNewPosition.X,
                proposedNewPosition.Y,
                (DefaultBoundingBoxSize.X * entity.Scale),
                (DefaultBoundingBoxSize.Y * entity.Scale));

            RectangleF correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            RectangleF intersection = map.GetIntersection(correctedBoundingBox);
            if (intersection != RectangleF.Empty)
            {
                return;                
            }
            entity.Position = proposedNewPosition;
            entity.BoundingBox = proposedNewBoundingBox;
            System.Diagnostics.Debug.WriteLine($"POS: X: {entity.Position.X}, Y: {entity.Position.Y}, BBOX: X: {entity.BoundingBox.X}, Y: {entity.BoundingBox.Y} ");
        }
    }
}
