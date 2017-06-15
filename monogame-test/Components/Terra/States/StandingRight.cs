using monogame_test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Components.Terra.States
{
    public class StandingRight : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            entity.Velocity = new Vector2(0, entity.Velocity.Y);
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
            else if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = TerraStates.JumpingRight;
            }
        }
    }
}
