using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Components
{
    public interface IComponent
    {
        void Update(GameTime deltaTime, Entity entity);
    }
}
