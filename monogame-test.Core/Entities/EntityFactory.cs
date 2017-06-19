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

        public async Task<Entity> CreateTerraEntity()
        {
            var terraInput = new TerraInputComponent();
            var terraGraphics = new TerraGraphicsComponent(_spriteSheetLoader, _entitySpriteBatch, terraInput);
            await terraGraphics.Load();
            var terraPhysics = new TerraPhysicsComponent(_mapManager);

            var terra = new Entity(terraInput, terraPhysics, terraGraphics);            
            terra.Scale = 4f;
            terra.Position = new Vector2(75, 75);
            EntityRegistry.Add(terra);
            PlayerEntity = terra;

            return terra;
        }

        public async Task<Entity> CreateTestNpcEntity()
        {
            var testNpcGraphics = new TestNpcGraphicsComponent(_spriteSheetLoader, _entitySpriteBatch);
            await testNpcGraphics.Load();
            var testNpcPhysics = new TestNpcPhysicsComponent(_mapManager);

            var testNpc = new Entity(testNpcPhysics, testNpcGraphics);
            testNpc.Scale = 4f;
            testNpc.Position = new Vector2(150, 40);
            EntityRegistry.Add(testNpc);
            return testNpc;
        }
    }
}
