using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Terra.States
{
    public class JumpingLeft : IEntityState
    {
        public void EnterState(Entity entity, IEntityState oldState)
        {
            if (!(oldState is JumpingRight))
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
                    entity.State = TerraStates.StandingLeft;
                }
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                entity.Velocity = new Vector2(-TerraInputComponent.DefaultVelocity, entity.Velocity.Y);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                entity.State = TerraStates.JumpingRight;
            }
        }
    }
}
