using monogame_test.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using monogame_test.Components.Terra.States;

namespace monogame_test.Components.Terra
{
    public class TerraInputComponent : IInputComponent
    {
        public const float DefaultVelocity = 150;

        public TerraInputComponent()
        {

        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            var keyboardState = Keyboard.GetState();

            if (entity.State == null)
            {
                entity.State = TerraStates.StandingDown;
            }

            IEntityState oldState = entity.State;

            entity.State.Update(entity, keyboardState);
            if (entity.State != null && entity.State != oldState)
            {
                entity.State.EnterState(entity);
            }            

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                entity.Velocity *= 1.5f;                
            }
        }
    }
}
