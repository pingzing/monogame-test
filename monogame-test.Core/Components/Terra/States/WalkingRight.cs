using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test.Core.Components.Terra.States
{
    public class WalkingRight : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            entity.Velocity = new Vector2(entity.Velocity.X + entity.HorizontalAcceleration, entity.Velocity.Y);
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
                entity.Velocity = new Vector2(Math.Min(TerraInputComponent.MaxHorizontalVelocity, entity.Velocity.X + entity.HorizontalAcceleration), 
                    entity.Velocity.Y);
            }
            else if (keyboard.IsKeyDown(Keys.W))
            {
                entity.State = TerraStates.WalkingUp;
            }           
            else if (entity.Velocity.X == 0)
            {
                entity.State = TerraStates.StandingRight;
            }            
        }
    }
}
