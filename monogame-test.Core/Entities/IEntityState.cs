using Microsoft.Xna.Framework.Input;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Entities
{
    public interface IEntityState
    {        
        void Update(Entity entity, KeyboardState keyboard);
        void EnterState(Entity entity, IEntityState oldState);
    }
}
