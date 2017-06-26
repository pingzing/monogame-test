using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Components
{
    public interface IAttackComponent : IComponent
    {
        void Draw(Entity entity, GameTime gameTime);
    }
}
