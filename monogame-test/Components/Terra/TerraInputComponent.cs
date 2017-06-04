using monogame_test.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace monogame_test.Components.Terra
{
    public class TerraInputComponent : IInputComponent
    {
        public const float DefaultVelocity = 150;

        public enum TerraState
        {
            StandingLeft,
            StandingRight,
            StandingDown,
            StandingUp,
            WalkingLeft,
            WalkingRight,
            WalkingDown,
            WalkingUp
        }

        public TerraState CurrentState { get; private set; }

        public TerraInputComponent()
        {

        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                CurrentState = TerraState.WalkingLeft;
                entity.XVelocity = -DefaultVelocity;
                entity.YVelocity = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                CurrentState = TerraState.WalkingDown;
                entity.XVelocity = 0;
                entity.YVelocity = DefaultVelocity;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                CurrentState = TerraState.WalkingRight;
                entity.XVelocity = DefaultVelocity;
                entity.YVelocity = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                CurrentState = TerraState.WalkingUp;
                entity.XVelocity = 0;
                entity.YVelocity = -DefaultVelocity;
            }
            else
            {
                entity.XVelocity = 0;
                entity.YVelocity = 0;
                if (CurrentState == TerraState.WalkingDown)
                {
                    CurrentState = TerraState.StandingDown;
                }
                else if (CurrentState == TerraState.WalkingLeft)
                {
                    CurrentState = TerraState.StandingLeft;
                }
                else if (CurrentState == TerraState.WalkingRight)
                {
                    CurrentState = TerraState.StandingRight;
                }
                else if (CurrentState == TerraState.WalkingUp)
                {
                    CurrentState = TerraState.StandingUp;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                entity.XVelocity *= 1.5f;
                entity.YVelocity *= 1.5f;
            }
        }
    }
}
