using Microsoft.Xna.Framework.Input;
using monogame_test.Entities;

namespace monogame_test.Entities
{
    public interface IEntityState
    {        
        void Update(Entity entity, KeyboardState keyboard);
        void EnterState(Entity entity, IEntityState oldState);
    }
}
