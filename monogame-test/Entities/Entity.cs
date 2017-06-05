using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Components;
using monogame_test.Maps;
using monogame_test.RenderHelpers;

namespace monogame_test.Entities
{
    public class Entity
    {
        private MapManager _mapManager;

        protected IInputComponent InputComponent;
        protected IGraphicsComponent GraphicsComponent;
        protected IPhysicsComponent PhysicsComponent;        

        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; } = 1.0f;        

        private RectangleF _boundingBox;
        public RectangleF BoundingBox
        {
            get { return _boundingBox; }
            set
            {
                _boundingBox = new RectangleF(
                    value.X - BoundingBoxOrigin.X, 
                    value.Y - BoundingBoxOrigin.Y, 
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

        public Entity(IGraphicsComponent graphics, IPhysicsComponent physics, IInputComponent input, MapManager mapManager)
        {
            GraphicsComponent = graphics;
            PhysicsComponent = physics;
            InputComponent = input;
            _mapManager = mapManager;
        }
        
        public virtual void Update(GameTime deltaTime)
        {
            InputComponent.Update(deltaTime, this);
            PhysicsComponent.Update(deltaTime, this, _mapManager.CurrentMap);
            GraphicsComponent.Update(deltaTime, this);            
        }
        
        public virtual void Draw(GameTime deltaTime)
        {
            GraphicsComponent.Draw(deltaTime, this);
        }        

        public RectangleF GetOriginCorrectedBoundingBox(RectangleF bbox)
        {
            return new RectangleF(bbox.X - BoundingBoxOrigin.X, bbox.Y - BoundingBoxOrigin.Y, bbox.Width, bbox.Height);
        }
    }
}
