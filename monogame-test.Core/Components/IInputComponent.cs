using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Components
{
    public interface IInputComponent
    {
        void Update(GameTime deltaTime, Entity entity);
    }
}
