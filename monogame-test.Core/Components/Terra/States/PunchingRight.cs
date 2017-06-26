﻿using monogame_test.Core.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monogame_test.Core.Components.Terra.States
{
    public class PunchingRight : IEntityState
    {
        private const int AttackMaxTicks = 34;
        private int _currentAttackTicks = 0;

        public void EnterState(Entity entity, IEntityState oldState)
        {
            if (entity.Velocity.X != 0)
            {
                entity.Velocity = new Vector2(entity.Velocity.X / 2, entity.Velocity.Y);
            }            
        }

        public void Update(Entity entity, KeyboardState keyboard)
        {
            if (_currentAttackTicks >= AttackMaxTicks)
            {
                if (entity.IsAirbone)
                {
                    entity.State = TerraStates.JumpingRight;
                    _currentAttackTicks = 0;
                }
                else
                {
                    entity.State = TerraStates.WalkingRight;
                    _currentAttackTicks = 0;
                }
            }

            _currentAttackTicks++;
        }
    }
}
