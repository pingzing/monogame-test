using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Components
{
    public interface IDrawableComponent : IComponent
    {
        void Draw(GameTime gameTime, Entity entity);
    }
}
