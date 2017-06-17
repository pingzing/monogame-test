using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test.Core.Components.Terra.States
{
    public class WalkingLeft : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            entity.Velocity = new Vector2(entity.Velocity.X - entity.HorizontalAcceleration, entity.Velocity.Y);
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = TerraStates.JumpingLeft;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                entity.Velocity = new Vector2(
                    Math.Max(entity.Velocity.X - entity.HorizontalAcceleration, -entity.MaxHorizontalVelocity), 
                    entity.Velocity.Y);
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
            else if (entity.Velocity.X == 0)
            {
                entity.State = TerraStates.StandingLeft;
            }            
        }
    }
}
