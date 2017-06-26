using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Player.States
{
    public class Punching : IEntityState
    {
        private const int AttackMaxTicks = 34;
        private int _currentAttackTicks = 0;

        public void EnterState(Entity entity, IEntityState oldState)
        {
            
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (_currentAttackTicks >= AttackMaxTicks)
            {
                if (entity.IsAirbone)
                {
                    entity.State = PlayerStates.Jumping;
                    _currentAttackTicks = 0;
                }
                else
                {
                    entity.State = PlayerStates.Walking;
                    _currentAttackTicks = 0;
                }
            }

            _currentAttackTicks++;
        }
    }
}
