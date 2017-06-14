using monogame_test.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Components.Terra.States
{
    public class WalkingRight : IEntityState
    {
        public void EnterState(Entity entity)
        {
            entity.Velocity = new Vector2(TerraInputComponent.DefaultVelocity, entity.Velocity.Y);
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                entity.State = TerraStates.WalkingLeft;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                entity.State = TerraStates.WalkingDown;
            }
            else if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = TerraStates.JumpingRight;
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
                entity.State = TerraStates.StandingRight;
            }
        }
    }
}
