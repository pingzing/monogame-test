using monogame_test.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Components.Terra.States
{
    public class WalkingLeft : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            entity.Velocity = new Vector2(-TerraInputComponent.DefaultVelocity, entity.Velocity.Y);
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = TerraStates.JumpingLeft;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                entity.State = TerraStates.WalkingLeft;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                entity.State = TerraStates.WalkingDown;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                entity.State = TerraStates.WalkingRight;
            }
            else if (keyboard.IsKeyDown(Keys.W))
            {
                entity.State = TerraStates.WalkingUp;
            }            
            else
            {
                entity.State = TerraStates.StandingLeft;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                entity.Velocity = new Vector2(-TerraInputComponent.DefaultVelocity * 1.5f, entity.Velocity.Y);
            }
            else
            {
                entity.Velocity = new Vector2(-TerraInputComponent.DefaultVelocity, entity.Velocity.Y);
            }
        }
    }
}
