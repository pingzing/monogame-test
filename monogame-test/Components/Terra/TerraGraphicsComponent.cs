using System;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Entities;
using TexturePackerLoader;
using monogame_test.AnimationSystem;
using Microsoft.Xna.Framework;
using TexturePackerMonoGameDefinitions;
using Microsoft.Xna.Framework.Input;

namespace monogame_test.Components.Terra
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

        private Animation _currentAnimation;
        private Texture2D _bBoxOutline;    

        public TerraGraphicsComponent(GraphicsDevice graphicsDevice, SpriteSheetLoader spriteSheetLoader,
            SpriteBatch spriteBatch, TerraInputComponent terraInput)
        {
            _terraInput = terraInput;
            _spriteBatch = spriteBatch;
            _spriteRender = new SpriteRender(_spriteBatch);
            _spriteSheetLoader = spriteSheetLoader;
            _terraSheet = spriteSheetLoader.Load("test-terra-atlas");            

            _standLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_08 });

            _standRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.FlipHorizontally,
                new String[] { TestTerraAtlas.Sprite_08 });

            _standUpAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_02 });

            _standDownAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_05 });         

            _walkLeftAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,                
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_08,
                     TestTerraAtlas.Sprite_09,
                     TestTerraAtlas.Sprite_08,
                     TestTerraAtlas.Sprite_07 });

            _walkRightAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200), 
                _terraSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { TestTerraAtlas.Sprite_08,
                     TestTerraAtlas.Sprite_09,
                     TestTerraAtlas.Sprite_08,
                     TestTerraAtlas.Sprite_07 });

            _walkUpAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_02,
                TestTerraAtlas.Sprite_03,
                TestTerraAtlas.Sprite_02,
                TestTerraAtlas.Sprite_01});

            _walkDownAnimation = new Animation(                
                TimeSpan.FromMilliseconds(200),
                _terraSheet,
                SpriteEffects.None,
                new string[] { TestTerraAtlas.Sprite_05,
                TestTerraAtlas.Sprite_06,
                TestTerraAtlas.Sprite_05,
                TestTerraAtlas.Sprite_04});                   
        }        

        public void Update(GameTime deltaTime, Entity entity)
        {            
            switch (_terraInput.CurrentState)
            {
                case TerraInputComponent.TerraState.StandingDown:
                    _currentAnimation = _standDownAnimation;
                    break;
                case TerraInputComponent.TerraState.StandingLeft:
                    _currentAnimation = _standLeftAnimation;
                    break;
                case TerraInputComponent.TerraState.StandingRight:
                    _currentAnimation = _standRightAnimation;
                    break;
                case TerraInputComponent.TerraState.StandingUp:
                    _currentAnimation = _standUpAnimation;
                    break;
                case TerraInputComponent.TerraState.WalkingLeft:
                    _currentAnimation = _walkLeftAnimation;
                    break;
                case TerraInputComponent.TerraState.WalkingDown:
                    _currentAnimation = _walkDownAnimation;
                    break;
                case TerraInputComponent.TerraState.WalkingRight:
                    _currentAnimation = _walkRightAnimation;
                    break;
                case TerraInputComponent.TerraState.WalkingUp:
                    _currentAnimation = _walkUpAnimation;
                    break;
            }

            TimeSpan newFrameTime = TimeSpan.FromMilliseconds(
                TerraInputComponent.DefaultVelocity /
                Math.Max(1, ((Math.Abs(entity.XVelocity) + Math.Abs(entity.YVelocity)) / TerraInputComponent.DefaultVelocity)));
            _currentAnimation.SetTimeBetweenFrames(newFrameTime);
            _currentAnimation.Update(deltaTime);
        }

        public void Draw(GameTime deltaTime, Entity entity)
        {
            var currentFrame = _currentAnimation.GetCurrentFrame();
            _spriteRender.Draw(currentFrame.SpriteFrame,
                new Vector2(entity.X, entity.Y),
                1,
                Color.White,
                rotation: 0,
                scale: 4,
                spriteEffects: currentFrame.SpriteEffect);

            _spriteBatch.Draw(Game.BBoxOutline, entity.BoundingBox, Color.White);
        }
    }
}
