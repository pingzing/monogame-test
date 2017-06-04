using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Components;

namespace monogame_test.Entities
{
    public class Entity
    {
        protected IInputComponent InputComponent;
        protected IGraphicsComponent GraphicsComponent;
        protected IPhysicsComponent PhysicsComponent;        

        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public float X { get; set; }
        public float Y { get; set; }        

        public Entity(IGraphicsComponent graphics, IPhysicsComponent physics, IInputComponent input)
        {
            GraphicsComponent = graphics;
            PhysicsComponent = physics;
            InputComponent = input;
        }
        
        public virtual void Update(GameTime deltaTime)
        {
            InputComponent.Update(deltaTime, this);
            PhysicsComponent.Update(deltaTime, this);
            GraphicsComponent.Update(deltaTime, this);            
        }
        
        public virtual void Draw(GameTime deltaTime)
        {
            GraphicsComponent.Draw(deltaTime, this);
        }        
    }
}
