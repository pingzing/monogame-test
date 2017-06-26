using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test.Core.Components.Player.States
{
    public class Jumping : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            if (!entity.IsAirbone)
            { 
                entity.Velocity = new Vector2(entity.Velocity.X, -350);
            }
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (entity.Velocity.Y == 0 
                && !entity.IsAirbone)
            {
                if (entity.Velocity.X != 0)
                {
                    entity.State = PlayerStates.Walking;
                }
                else
                {
                    entity.State = PlayerStates.Standing;
                }
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                if (entity.CurrentFacing != Facing.Left)
                {
                    entity.CurrentFacing = Facing.Left;
                }

                entity.Velocity = new Vector2(
                    Math.Max(entity.Velocity.X - entity.HorizontalAcceleration, -entity.MaxHorizontalVelocity),
                    entity.Velocity.Y);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                if (entity.CurrentFacing != Facing.Right)
                {
                    entity.CurrentFacing = Facing.Right;
                }

                entity.Velocity = new Vector2(
                    Math.Min(entity.MaxHorizontalVelocity, entity.Velocity.X + entity.HorizontalAcceleration),
                    entity.Velocity.Y);
            }            
        }
    }
}
