using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Components;
using monogame_test.Maps;

namespace monogame_test.Entities
{
    public class Entity
    {
        private MapManager _mapManager;

        protected IInputComponent InputComponent;
        protected IGraphicsComponent GraphicsComponent;
        protected IPhysicsComponent PhysicsComponent;        

        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Rectangle BoundingBox { get; set; }

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
    }
}
