using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Entities;
using TexturePackerLoader;

namespace monogame_test.Components
{
    public interface IGraphicsComponent
    {        
        void Update(GameTime deltaTime, Entity entity);
        void Draw(GameTime deltaTime, Entity entity);
    }
}
