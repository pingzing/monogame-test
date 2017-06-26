using monogame_test.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Terra.States
{
    public class StandingLeft : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                entity.State = TerraStates.WalkingLeft;                
            }            
            else if (keyboard.IsKeyDown(Keys.D))
            {
                entity.State = TerraStates.WalkingRight;                
            }
            else if (keyboard.IsKeyDown(Keys.X))
            {
                entity.State = TerraStates.PunchingLeft;
            }
            else if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = TerraStates.JumpingLeft;
            }
        }
    }
}
