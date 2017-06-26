using monogame_test.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Player.States
{
    public class Standing : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                entity.CurrentFacing = Facing.Left;
                entity.State = PlayerStates.Walking;                
            }            
            else if (keyboard.IsKeyDown(Keys.D))
            {
                entity.CurrentFacing = Facing.Right;
                entity.State = PlayerStates.Walking;                
            }
            else if (keyboard.IsKeyDown(Keys.X))
            {
                entity.State = PlayerStates.Punching;
            }
            else if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = PlayerStates.Jumping;
            }
        }
    }
}
