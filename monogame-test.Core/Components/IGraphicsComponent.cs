using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Entities;
using TexturePackerLoader;

namespace monogame_test.Core.Components
{
    public interface IGraphicsComponent
    {        
        void Update(GameTime deltaTime, Entity entity);
        void Draw(GameTime deltaTime, Entity entity);
    }
}
