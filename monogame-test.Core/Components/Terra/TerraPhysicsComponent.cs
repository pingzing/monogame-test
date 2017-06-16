using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monogame_test.Core.Entities;
using Microsoft.Xna.Framework;
using monogame_test.Core.Maps;
using monogame_test.Core.RenderHelpers;

namespace monogame_test.Core.Components.Terra
{
    class TerraPhysicsComponent : IPhysicsComponent
    {
        private Point DefaultBoundingBoxSize = new Point(12, 10);
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

            // Apply gravity if airborne
            if (!map.IsStandingOnGround(entity.BoundingBox))
            {
                entity.Velocity = new Vector2(entity.Velocity.X, entity.Velocity.Y + Gravity);
            }

            // Step X position
            Vector2 proposedNewPosition = new Vector2(entity.Velocity.X * delta + entity.Position.X, entity.Position.Y);
            RectangleF proposedNewBoundingBox = new RectangleF(
                proposedNewPosition.X,
                proposedNewPosition.Y,
                (DefaultBoundingBoxSize.X * entity.Scale),
                (DefaultBoundingBoxSize.Y * entity.Scale));

            // Check X-axis collision
            RectangleF correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            bool horizontalCollidedWith = map.IsColliding(correctedBoundingBox);
            if (horizontalCollidedWith)
            {
                float xCorrection = GetXCorrection(proposedNewPosition, correctedBoundingBox, map, entity);
                proposedNewPosition = new Vector2(proposedNewPosition.X + xCorrection, proposedNewPosition.Y);
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
            }

            // Step Y position
            proposedNewPosition = new Vector2(proposedNewPosition.X, entity.Velocity.Y * delta + entity.Position.Y);
            proposedNewBoundingBox = proposedNewBoundingBox = new RectangleF(
                proposedNewPosition.X,
                proposedNewPosition.Y,
                (DefaultBoundingBoxSize.X * entity.Scale),
                (DefaultBoundingBoxSize.Y * entity.Scale));

            // Check Y-axis collision
            correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            bool verticalCollidedWith = map.IsColliding(correctedBoundingBox);
            if (verticalCollidedWith)
            {
                float yCorrection = GetYCorrection(proposedNewPosition, correctedBoundingBox, map, entity);
                proposedNewPosition = new Vector2(proposedNewPosition.X, proposedNewPosition.Y + yCorrection);
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
            }

            // Final assignment of bounding box
            proposedNewBoundingBox = new RectangleF(proposedNewPosition.X,
                proposedNewPosition.Y,
                (DefaultBoundingBoxSize.X * entity.Scale),
                (DefaultBoundingBoxSize.Y * entity.Scale));

            entity.Position = proposedNewPosition;
            entity.BoundingBox = proposedNewBoundingBox;            
        }

        private float GetXCorrection(Vector2 proposedNewPosition, RectangleF correctedBoundingBox, TestMap map, Entity entity)
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

            return xCorrection;
        }

        private float GetYCorrection(Vector2 proposedNewPosition, RectangleF correctedBoundingBox, TestMap map, Entity entity)
        {
            float yCorrection = 0f;

            // Going down
            if (proposedNewPosition.Y > entity.Position.Y)
            {
                float bboxYCoordinate = correctedBoundingBox.Bottom;
                float bboxMiddleX = correctedBoundingBox.Center.X;
                RectangleF obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, true);
                yCorrection = (Math.Abs(bboxYCoordinate - obstacleBox.Top) + 1 ) * -1;
            }
            // Going up
            else if (proposedNewPosition.Y < entity.Position.Y)
            {
                float bboxYCoordinate = correctedBoundingBox.Top;
                float bboxMiddleX = correctedBoundingBox.Center.X;
                RectangleF obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, false);
                yCorrection = Math.Abs(bboxYCoordinate - obstacleBox.Bottom) + 1;
            }

            return yCorrection;
        }
    }
}
