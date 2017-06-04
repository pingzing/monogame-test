using Microsoft.Xna.Framework.Graphics;
using monogame_test.Components.Terra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.Entities
{
    public class EntityFactory
    {
        private GraphicsDevice _graphics;
        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteBatch _entitySpriteBatch;

        public List<Entity> EntityRegistry { get; private set; }

        public EntityFactory(GraphicsDevice graphics,
            SpriteSheetLoader spriteSheetLoader, 
            SpriteBatch entitySpriteBatch)
        {
            _graphics = graphics;
            _spriteSheetLoader = spriteSheetLoader;
            _entitySpriteBatch = entitySpriteBatch;
            EntityRegistry = new List<Entity>();
        }

        public Entity CreateTerraEntity()
        {
            var terraInput = new TerraInputComponent();
            var terraGraphics = new TerraGraphicsComponent(_graphics, _spriteSheetLoader, _entitySpriteBatch, terraInput);
            var terraPhysics = new TerraPhysicsComponent();

            var terra = new Entity(terraGraphics,
                terraPhysics,
                terraInput);
            EntityRegistry.Add(terra);
            return terra;
        }
    }
}
