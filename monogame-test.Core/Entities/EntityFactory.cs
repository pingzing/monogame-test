using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Components.Terra;
using monogame_test.Core.Components.TestNpc;
using monogame_test.Core.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.Core.Entities
{
    public class EntityFactory
    {
        private GraphicsDevice _graphics;
        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteBatch _entitySpriteBatch;
        private MapManager _mapManager;
        public Entity PlayerEntity { get; private set; }

        public List<Entity> EntityRegistry { get; private set; }

        public EntityFactory(GraphicsDevice graphics,
            SpriteSheetLoader spriteSheetLoader, 
            SpriteBatch entitySpriteBatch,
            MapManager mapManager)
        {
            _graphics = graphics;
            _spriteSheetLoader = spriteSheetLoader;
            _entitySpriteBatch = entitySpriteBatch;
            _mapManager = mapManager;
            EntityRegistry = new List<Entity>();
        }

        public Entity CreateTerraEntity()
        {
            var terraInput = new TerraInputComponent();
            var terraGraphics = new TerraGraphicsComponent(_graphics, _spriteSheetLoader, _entitySpriteBatch, terraInput);
            var terraPhysics = new TerraPhysicsComponent(_mapManager);

            var terra = new Entity(terraInput, terraPhysics, terraGraphics);
            terra.Scale = 4f;
            terra.Position = new Vector2(75, 75);
            EntityRegistry.Add(terra);
            PlayerEntity = terra;

            return terra;
        }

        public Entity CreateTestNpcEntity()
        {
            var testNpcGraphics = new TestNpcGraphicsComponent();
            var testNpcPhysics = new TestNpcPhysicsComponent();

            var testNpc = new Entity(testNpcPhysics, testNpcGraphics);
            testNpc.Position = new Vector2(150, 40);
            EntityRegistry.Add(testNpc);
            return testNpc;
        }
    }
}
