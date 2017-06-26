using monogame_test.Core.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using monogame_test.Core.Components.Terra.States;
using System;
using System.Linq;

namespace monogame_test.Core.Components.Player
{
    public class PlayerInputComponent : IInputComponent
    {
        private EntityFactory _factory;

        public PlayerInputComponent(EntityFactory factory)
        {
            _factory = factory;
        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            var keyboardState = Keyboard.GetState();   
            
            if (keyboardState.IsKeyDown(Keys.Z))
            {
                Entity interactedEntity = _factory
                    .GetIntersectingEntities(entity.BoundingBox)
                    .FirstOrDefault(x => x.Id != entity.Id);
                if (interactedEntity != null)
                {
                    IDialogComponent dialog = interactedEntity.Components
                        .OfType<IDialogComponent>()
                        .FirstOrDefault();
                    if (dialog != null)
                    {
                        dialog.ProgressDialogue();
                    }
                }
            }

            if (entity.State == null)
            {
                entity.State = PlayerStates.Standing;
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
