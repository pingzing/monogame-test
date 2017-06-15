using monogame_test.Entities;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Components.Terra.States
{
    public class JumpingRight : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            if (!(oldState is JumpingLeft))
            {
                entity.Velocity = new Vector2(entity.Velocity.X, -350);
            }
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (entity.Velocity.Y == 0)
            {
                if (entity.Velocity.X < 0)
                {
                    entity.State = TerraStates.WalkingLeft;
                }
                if (entity.Velocity.X > 0)
                {
                    entity.State = TerraStates.WalkingRight;
                }
                else
                {
                    entity.State = TerraStates.StandingRight;
                }
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                entity.State = TerraStates.JumpingLeft;
            }
        }
    }
}
