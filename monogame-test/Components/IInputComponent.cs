using Microsoft.Xna.Framework;
using monogame_test.Entities;

namespace monogame_test.Components
{
    public interface IInputComponent
    {
        void Update(GameTime deltaTime, Entity entity);
    }
}
