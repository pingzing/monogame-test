using monogame_test.Core.Entities;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Terra.States
{
    public class WalkingDown : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            entity.Velocity = new Vector2(0, TerraInputComponent.DefaultVelocity);
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
                entity.State = TerraStates.StandingDown;
            }
        }
    }
}
