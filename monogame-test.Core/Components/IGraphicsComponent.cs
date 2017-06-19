using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;

namespace monogame_test.Core.Components
{
    public interface IGraphicsComponent : IComponent
    {                
        void Draw(GameTime deltaTime, Entity entity);
    }
}
