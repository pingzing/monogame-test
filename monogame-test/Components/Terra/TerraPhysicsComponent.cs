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
        private bool IsAirborne = true;
        private float Gravity = 9.8f;

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

            Vector2 oldPos = new Vector2(entity.Position.X, entity.Position.Y);
            RectangleF oldBbox = new RectangleF(entity.BoundingBox);

            IsAirborne = !map.IsStandingOnGround(entity.BoundingBox);
            if (IsAirborne)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, entity.Velocity.Y + Gravity);
            }

            Vector2 proposedNewPosition = new Vector2(entity.Velocity.X * delta + entity.Position.X, entity.Velocity.Y * delta + entity.Position.Y);
            RectangleF proposedNewBoundingBox = new RectangleF(
                proposedNewPosition.X,
                proposedNewPosition.Y,
                (DefaultBoundingBoxSize.X * entity.Scale),
                (DefaultBoundingBoxSize.Y * entity.Scale));

            // Check X-axis collision
            RectangleF correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            bool collidedWith = map.IsColliding(correctedBoundingBox);
            if (collidedWith)
            {
                float xCorrection = 0f;        
                
                // Going right
                if (proposedNewPosition.X > entity.Position.X)
                {
                    float bboxXCoordinate = correctedBoundingBox.Right;
                    float bboxMiddleY = correctedBoundingBox.Center.Y;
                    RectangleF obstacleBbox = map.GetNearestHorizontalCollidedObject(bboxXCoordinate, bboxMiddleY, true);
                    xCorrection = (Math.Abs(bboxXCoordinate - obstacleBbox.Left) + 1) * -1;                    

                }
                //going left
                else if (proposedNewPosition.X < entity.Position.X)
                {
                    float bboxXCoordinate = correctedBoundingBox.Left;
                    float bboxMiddleY = correctedBoundingBox.Center.Y;
                    RectangleF obstacleBbox = map.GetNearestHorizontalCollidedObject(bboxXCoordinate, bboxMiddleY, false);
                    xCorrection = Math.Abs(bboxXCoordinate - obstacleBbox.Right) + 1;
                }

                proposedNewPosition = new Vector2(proposedNewPosition.X + xCorrection, proposedNewPosition.Y);
                proposedNewBoundingBox = new RectangleF
                    (proposedNewPosition.X,
                    proposedNewPosition.Y,
                    (DefaultBoundingBoxSize.X * entity.Scale),
                    (DefaultBoundingBoxSize.Y * entity.Scale));

                entity.Velocity = new Vector2(0, entity.Velocity.Y);
            }

            // Next, check Y-axis collision
            correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            collidedWith = map.IsColliding(correctedBoundingBox);
            if (collidedWith)
            {                
                float yCorrection = 0f;

                // Going down
                if (proposedNewPosition.Y > entity.Position.Y)
                {
                    float bboxYCoordinate = correctedBoundingBox.Bottom;
                    float bboxMiddleX = correctedBoundingBox.Center.X;
                    RectangleF obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, true);
                    yCorrection = (Math.Abs(bboxYCoordinate - obstacleBox.Top) + 1) * -1;
                }
                // Going up
                else if (proposedNewPosition.Y < entity.Position.Y)
                {
                    float bboxYCoordinate = correctedBoundingBox.Top;
                    float bboxMiddleX = correctedBoundingBox.Center.X;
                    RectangleF obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, false);
                    yCorrection = Math.Abs(bboxYCoordinate - obstacleBox.Bottom) + 1;
                }                

                proposedNewPosition = new Vector2(proposedNewPosition.X, proposedNewPosition.Y + yCorrection);
                proposedNewBoundingBox = new RectangleF
                    (proposedNewPosition.X,
                    proposedNewPosition.Y,
                    (DefaultBoundingBoxSize.X * entity.Scale),
                    (DefaultBoundingBoxSize.Y * entity.Scale));

                entity.Velocity = new Vector2(entity.Velocity.X, 0);
            }
                           
            entity.Position = proposedNewPosition;
            entity.BoundingBox = proposedNewBoundingBox;
            if (oldBbox != entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox) || oldPos != proposedNewPosition)
            {
                System.Diagnostics.Debug.WriteLine($"POS: X: {entity.Position.X}\t Y: {entity.Position.Y}\t BBOX: X: {entity.BoundingBox.X} Y: {entity.BoundingBox.Y} ");
            }
        }
    }
}
