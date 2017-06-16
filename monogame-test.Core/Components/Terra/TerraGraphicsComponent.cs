using System;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Entities;
using TexturePackerLoader;
using monogame_test.Core.AnimationSystem;
using Microsoft.Xna.Framework;
using TexturePackerMonoGameDefinitions;
using Microsoft.Xna.Framework.Input;
using monogame_test.Core.RenderHelpers;
using monogame_test.Core.Components.Terra.States;

namespace monogame_test.Core.Components.Terra
{
    public class TerraGraphicsComponent : IGraphicsComponent
    {
        private TerraInputComponent _terraInput;
        private SpriteBatch _spriteBatch;
        private SpriteRender _spriteRender;
        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteSheet _terraSheet;

        private Animation _standLeftAnimation;
        private Animation _standRightAnimation;
        private Animation _standUpAnimation;
        private Animation _standDownAnimation;    

        private Animation _walkLeftAnimation;
        private Animation _walkRightAnimation;
        private Animation _walkUpAnimation;
        private Animation _walkDownAnimation;

        private Animation _jumpLeftAnimation;
        private Animation _jumpRightAnimation;

        private Animation _currentAnimation;           

        public TerraGraphicsComponent(GraphicsDevice graphicsDevice, SpriteSheetLoader spriteSheetLoader,
            SpriteBatch spriteBatch, TerraInputComponent terraInput)
        {
            _terraInput = terraInput;
            _spriteBatch = spriteBatch;
            _spriteRender = new SpriteRender(_spriteBatch);
            _spriteSheetLoader = spriteSheetLoader;
            _terraSheet = spriteSheetLoader.Load("TestTerraAtlas");            

            _standLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandLeft });

            _standRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.FlipHorizontally,
                new String[] { TestTerraAtlas.TerraStandLeft });

            _standUpAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandUp });

            _standDownAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandDown });         

            _walkLeftAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,                
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandLeft,
                     TestTerraAtlas.TerraWalkLeft02,
                     TestTerraAtlas.TerraStandLeft,
                     TestTerraAtlas.TerraWalkLeft01 });

            _walkRightAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200), 
                _terraSheet,
                SpriteEffects.FlipHorizontally,
                new string[] {  TestTerraAtlas.TerraStandLeft,
                     TestTerraAtlas.TerraWalkLeft02,
                     TestTerraAtlas.TerraStandLeft,
                     TestTerraAtlas.TerraWalkLeft01 });

            _walkUpAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandUp,
                TestTerraAtlas.TerraWalkUp01,
                TestTerraAtlas.TerraStandUp,
                TestTerraAtlas.TerraWalkUp02});

            _walkDownAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraStandDown,
                TestTerraAtlas.TerraWalkDown01,
                TestTerraAtlas.TerraStandDown,
                TestTerraAtlas.TerraWalkDown02});

            _jumpRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { TestTerraAtlas.TerraArmRaise });

            _jumpLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.TerraArmRaise });
        }        

        public void Update(GameTime deltaTime, Entity entity)
        {                     
            if (entity.State is StandingDown)
            {
                _currentAnimation = _standDownAnimation;
            }
            else if (entity.State is StandingLeft)
            {
                _currentAnimation = _standLeftAnimation;
            }
            else if (entity.State is StandingRight)
            {
                _currentAnimation = _standRightAnimation;
            }
            else if (entity.State is StandingUp)
            {
                _currentAnimation = _standUpAnimation;
            }
            else if (entity.State is WalkingLeft)
            {
                _currentAnimation = _walkLeftAnimation;
            }
            else if (entity.State is WalkingDown)
            {
                _currentAnimation = _walkDownAnimation;
            }
            else if (entity.State is WalkingRight)
            {
                _currentAnimation = _walkRightAnimation;
            }
            else if (entity.State is WalkingUp)
            {
                _currentAnimation = _walkUpAnimation;
            }
            else if (entity.State is JumpingLeft)
            {
                _currentAnimation = _jumpLeftAnimation;
            }
            else if (entity.State is JumpingRight)
            {
                _currentAnimation = _jumpRightAnimation;
            }           

            TimeSpan newFrameTime = TimeSpan.FromMilliseconds(
                TerraInputComponent.DefaultVelocity /
                Math.Max(1, ((Math.Abs(entity.Velocity.X) + Math.Abs(entity.Velocity.Y)) / TerraInputComponent.DefaultVelocity)));
            _currentAnimation.SetTimeBetweenFrames(newFrameTime);
            _currentAnimation.Update(deltaTime);

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
                1,
                Color.White,
                rotation: 0,
                scale: entity.Scale,
                spriteEffects: currentFrame.SpriteEffect);

            //BoundingBoxHelper.DrawRectangle(entity.BoundingBox, Game.BBoxOutline, Color.White, _spriteBatch, false, 1);
        }
    }
}
