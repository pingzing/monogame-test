using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test.Core.Components.Player.States
{
    public class Walking : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            if (entity.CurrentFacing == Facing.Right)
            {
                entity.Velocity = new Vector2(entity.Velocity.X + entity.HorizontalAcceleration, entity.Velocity.Y);
            }
            else if (entity.CurrentFacing == Facing.Left
                || entity.CurrentFacing == Facing.Down
                || entity.CurrentFacing == Facing.Up)
            {
                entity.Velocity = new Vector2(entity.Velocity.X - entity.HorizontalAcceleration, entity.Velocity.Y);
            }
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.X))
            {
                entity.State = PlayerStates.Punching;
            }
            else if (keyboard.IsKeyDown(Keys.Space))
            {
                entity.State = PlayerStates.Jumping;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                entity.CurrentFacing = Facing.Left;
                entity.Velocity = new Vector2(
                    Math.Max(entity.Velocity.X - entity.HorizontalAcceleration, -entity.MaxHorizontalVelocity),
                    entity.Velocity.Y);
            }                        
            else if (keyboard.IsKeyDown(Keys.D))
            {
                entity.CurrentFacing = Facing.Right;
                entity.Velocity = new Vector2(Math.Min(entity.MaxHorizontalVelocity, entity.Velocity.X + entity.HorizontalAcceleration), 
                    entity.Velocity.Y);
            }                              
            else if (entity.Velocity.X == 0)
            {
                entity.State = PlayerStates.Standing;
            }            
        }
    }
}
