using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using TexturePackerLoader;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.AnimationSystem;
using TexturePackerMonoGameDefinitions;

namespace monogame_test.Core.Components.TestNpc
{
    class TestNpcGraphicsComponent : IGraphicsComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteRender _spriteRender;
        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteSheet _testNpcSheet;

        private Animation _standLeftAnimation;

        private Animation _currentAnimation;

        public TestNpcGraphicsComponent(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteSheetLoader = spriteSheetLoader;
            _spriteRender = new SpriteRender(_spriteBatch);
        }

        public async Task Load()
        {
            _testNpcSheet = await _spriteSheetLoader.LoadAsync("TestTerraAtlas");

            _standLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(1000),
                _testNpcSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandLeft });
        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            if (_currentAnimation != _standLeftAnimation)
            {
                _currentAnimation = _standLeftAnimation;
            }

            Vector2 currentFrameOrigin = _currentAnimation.GetCurrentFrame().SpriteFrame.Origin;
            if (entity.BoundingBoxOrigin == default(Vector2))
            {
                entity.BoundingBoxOrigin = new Vector2(currentFrameOrigin.X, currentFrameOrigin.Y - 12);
            }
        }

        public void Draw(GameTime deltaTime, Entity entity)
        {
            var currentFrame = _currentAnimation.GetCurrentFrame();
            _spriteRender.Draw(currentFrame.SpriteFrame,
                new Vector2(entity.Position.X, entity.Position.Y),
                layerDepth: 1,
                color: Color.White,
                rotation: 0,
                scale: entity.Scale,
                spriteEffects: currentFrame.SpriteEffect);
        }
    }
}
