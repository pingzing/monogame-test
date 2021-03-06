﻿using System;
using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using monogame_test.Core.Maps;

namespace monogame_test.Core.Components.TestNpc
{
    public class TestNpcPhysicsComponent : IPhysicsComponent
    {
        private MapManager _mapManager;
        private Point DefaultBoundingBoxSize = new Point(12, 10);
        private float Gravity = 9.8f;

        public TestNpcPhysicsComponent(MapManager mapManager)
        {
            _mapManager = mapManager;
        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            //Initialize bounding box
            if (entity.BoundingBox == Rectangle.Empty)
            {
                entity.BoundingBox = new Rectangle(
                    (int)entity.Position.X,
                    (int)entity.Position.Y,
                    (int)(DefaultBoundingBoxSize.X * entity.Scale),
                    (int)(DefaultBoundingBoxSize.Y * entity.Scale));
            }

            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;

            // Apply gravity if airborne
            if (!_mapManager.CurrentMap.IsStandingOnGround(entity.BoundingBox))
            {
                entity.IsAirbone = true;
                entity.Velocity = new Vector2(entity.Velocity.X, entity.Velocity.Y + Gravity);
            }
            else
            {
                entity.IsAirbone = false;
            }

            // Step X position
            Vector2 proposedNewPosition = new Vector2(entity.Velocity.X * delta + entity.Position.X, entity.Position.Y);
            Rectangle proposedNewBoundingBox = new Rectangle(
                (int)proposedNewPosition.X,
                (int)proposedNewPosition.Y,
                (int)(DefaultBoundingBoxSize.X * entity.Scale),
                (int)(DefaultBoundingBoxSize.Y * entity.Scale));

            // Check X-axis collision
            Rectangle correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            bool horizontalCollidedWith = _mapManager.CurrentMap.IsColliding(correctedBoundingBox);
            if (horizontalCollidedWith)
            {
                float xCorrection = GetXCorrection(proposedNewPosition, correctedBoundingBox, _mapManager.CurrentMap, entity);
                proposedNewPosition = new Vector2(proposedNewPosition.X + xCorrection, proposedNewPosition.Y);
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
            }

            // Step Y position
            proposedNewPosition = new Vector2(proposedNewPosition.X, entity.Velocity.Y * delta + entity.Position.Y);
            proposedNewBoundingBox = proposedNewBoundingBox = new Rectangle(
                (int)proposedNewPosition.X,
                (int)proposedNewPosition.Y,
                (int)(DefaultBoundingBoxSize.X * entity.Scale),
                (int)(DefaultBoundingBoxSize.Y * entity.Scale));

            // Check Y-axis collision
            correctedBoundingBox = entity.GetOriginCorrectedBoundingBox(proposedNewBoundingBox);
            bool verticalCollidedWith = _mapManager.CurrentMap.IsColliding(correctedBoundingBox);
            if (verticalCollidedWith)
            {
                float yCorrection = GetYCorrection(proposedNewPosition, correctedBoundingBox, _mapManager.CurrentMap, entity);
                proposedNewPosition = new Vector2(proposedNewPosition.X, proposedNewPosition.Y + yCorrection);
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
            }

            // Final assignment of bounding box
            proposedNewBoundingBox = new Rectangle(
                (int)proposedNewPosition.X,
                (int)proposedNewPosition.Y,
                (int)(DefaultBoundingBoxSize.X * entity.Scale),
                (int)(DefaultBoundingBoxSize.Y * entity.Scale));

            entity.Position = proposedNewPosition;
            entity.BoundingBox = proposedNewBoundingBox;
        }

        private float GetXCorrection(Vector2 proposedNewPosition, Rectangle correctedBoundingBox, MapBase map, Entity entity)
        {
            float xCorrection = 0f;

            // Going right
            if (proposedNewPosition.X > entity.Position.X)
            {
                float bboxXCoordinate = correctedBoundingBox.Right;
                float bboxMiddleY = correctedBoundingBox.Center.Y;
                Rectangle obstacleBbox = map.GetNearestHorizontalCollidedObject(bboxXCoordinate, bboxMiddleY, true);
                xCorrection = (Math.Abs(bboxXCoordinate - obstacleBbox.Left) + 1) * -1;

            }
            //going left
            else if (proposedNewPosition.X < entity.Position.X)
            {
                float bboxXCoordinate = correctedBoundingBox.Left;
                float bboxMiddleY = correctedBoundingBox.Center.Y;
                Rectangle obstacleBbox = map.GetNearestHorizontalCollidedObject(bboxXCoordinate, bboxMiddleY, false);
                xCorrection = Math.Abs(bboxXCoordinate - obstacleBbox.Right) + 1;
            }

            return xCorrection;
        }

        private float GetYCorrection(Vector2 proposedNewPosition, Rectangle correctedBoundingBox, MapBase map, Entity entity)
        {
            float yCorrection = 0f;

            // Going down
            if (proposedNewPosition.Y > entity.Position.Y)
            {
                float bboxYCoordinate = correctedBoundingBox.Bottom;
                float bboxMiddleX = correctedBoundingBox.Center.X;
                Rectangle obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, true);
                yCorrection = (Math.Abs(bboxYCoordinate - obstacleBox.Top) + 1) * -1;
            }
            // Going up
            else if (proposedNewPosition.Y < entity.Position.Y)
            {
                float bboxYCoordinate = correctedBoundingBox.Top;
                float bboxMiddleX = correctedBoundingBox.Center.X;
                Rectangle obstacleBox = map.GetNearestVerticalCollidedObject(bboxYCoordinate, bboxMiddleX, false);
                yCorrection = Math.Abs(bboxYCoordinate - obstacleBox.Bottom) + 1;
            }

            return yCorrection;
        }
    }
}
