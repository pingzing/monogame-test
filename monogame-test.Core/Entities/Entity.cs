using Microsoft.Xna.Framework;
using monogame_test.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace monogame_test.Core.Entities
{
    public class Entity
    {
        public Guid Id { get; private set; }
        public List<IComponent> Components { get; set; } = new List<IComponent>();
        public IEntityState State { get; set; }
        public float HorizontalAcceleration { get; set; }
        public float MaxHorizontalVelocity { get; set; }
        public bool IsAirbone { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; } = 1.0f;        

        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
            set
            {
                _boundingBox = new Rectangle(
                    (int)(value.X - BoundingBoxOrigin.X), 
                    (int)(value.Y - BoundingBoxOrigin.Y), 
                    value.Width, 
                    value.Height);                
            }
        }

        private Vector2 _boundingBoxOrigin = Vector2.Zero;
        public Vector2 BoundingBoxOrigin
        {
            get { return _boundingBoxOrigin; }
            set { _boundingBoxOrigin = value * Scale; }
        }

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Entity(params IComponent[] components)
        {
            Id = Guid.NewGuid();
            Components = new List<IComponent>(components);
        }
        
        public virtual void Update(GameTime deltaTime)
        {
            foreach(var component in Components)
            {
                component.Update(deltaTime, this);
            }
        }
        
        public virtual void Draw(GameTime deltaTime)
        {
            foreach(var graphicsComponent in Components.OfType<IGraphicsComponent>())
            {
                graphicsComponent.Draw(deltaTime, this);
            }
        }        

        public Rectangle GetOriginCorrectedBoundingBox(Rectangle bbox)
        {
            return new Rectangle(
                (int)(bbox.X - BoundingBoxOrigin.X), 
                (int)(bbox.Y - BoundingBoxOrigin.Y), 
                bbox.Width, 
                bbox.Height);
        }
    }
}
