using monogame_test.Core.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using monogame_test.Core.Components.Terra.States;
using System;

namespace monogame_test.Core.Components.Terra
{
    public class TerraInputComponent : IInputComponent
    {        
        public TerraInputComponent()
        {

        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            var keyboardState = Keyboard.GetState();

            if (entity.State == null)
            {
                entity.State = TerraStates.StandingDown;
            }

            IEntityState oldState = entity.State;

            entity.State.Update(entity, keyboardState);
            if (entity.State != null && entity.State != oldState)
            {
                entity.State.EnterState(entity, oldState);
            }            
        }
    }
}
