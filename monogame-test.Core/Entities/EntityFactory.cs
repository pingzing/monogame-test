using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Components.Player;
using monogame_test.Core.Components.TestNpc;
using monogame_test.Core.DialogueSystem;
using monogame_test.Core.Maps;
using System.Collections.Generic;
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
        private DialogueManager _dialogueManager;

        public Entity PlayerEntity { get; private set; }

        public List<Entity> EntityRegistry { get; private set; }

        public EntityFactory(GraphicsDevice graphics,
            SpriteSheetLoader spriteSheetLoader, 
            SpriteBatch entitySpriteBatch,
            MapManager mapManager,
            DialogueManager dialogueManager)
        {
            _graphics = graphics;
            _spriteSheetLoader = spriteSheetLoader;
            _entitySpriteBatch = entitySpriteBatch;
            _mapManager = mapManager;
            _dialogueManager = dialogueManager;
            EntityRegistry = new List<Entity>();
        }

        public async Task<Entity> CreatePlayerEntity()
        {
            var playerInput = new PlayerInputComponent(this);
            var playerGraphics = new PlayerGraphicsComponent(_spriteSheetLoader, _entitySpriteBatch, playerInput);
            await playerGraphics.LoadAsync();
            var playerPhysics = new PlayerPhysicsComponent(_mapManager);

            var player = new Entity(playerInput, playerPhysics, playerGraphics);            
            player.Position = new Vector2(75, 75);
            EntityRegistry.Add(player);
            PlayerEntity = player;

            return player;
        }

        public async Task<Entity> CreateTestNpcEntity()
        {
            var testNpcGraphics = new TestNpcGraphicsComponent(_spriteSheetLoader, _entitySpriteBatch);
            await testNpcGraphics.LoadAsync();
            var testNpcPhysics = new TestNpcPhysicsComponent(_mapManager);
            var testNpcDialog = new TestNpcDialogComponent(_dialogueManager);

            var testNpc = new Entity(testNpcPhysics, testNpcGraphics, testNpcDialog);
            testNpc.Scale = 4f;
            testNpc.Position = new Vector2(150, 40);
            EntityRegistry.Add(testNpc);
            return testNpc;
        }

        public IEnumerable<Entity> GetIntersectingEntities(Rectangle boundingBox)
        {
            foreach (var entity in EntityRegistry)
            {
                if (entity.BoundingBox.Intersects(boundingBox))
                {
                    yield return entity;
                }
            }
        }
    }
}
