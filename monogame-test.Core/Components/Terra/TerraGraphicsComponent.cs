using System;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Entities;
using TexturePackerLoader;
using monogame_test.Core.AnimationSystem;
using Microsoft.Xna.Framework;
using TexturePackerMonoGameDefinitions;
using monogame_test.Core.RenderHelpers;
using monogame_test.Core.Components.Terra.States;
using monogame_test.Core.Content;
using monogame_test.Core.DebugHelpers;
using System.Threading.Tasks;

namespace monogame_test.Core.Components.Terra
{
    public class TerraGraphicsComponent : IGraphicsComponent
    {
        private TerraInputComponent _terraInput;
        private SpriteBatch _spriteBatch;
        private SpriteRender _spriteRender;
        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteSheet _stickSheet;

        private Animation _standLeftAnimation;
        private Animation _standRightAnimation;
        private Animation _standUpAnimation;
        private Animation _standDownAnimation;    

        private Animation _walkLeftAnimation;
        private Animation _walkRightAnimation;        

        private Animation _jumpLeftAnimation;
        private Animation _jumpRightAnimation;

        private Animation _currentAnimation;           

        public TerraGraphicsComponent(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch, 
            TerraInputComponent terraInput)
        {
            _terraInput = terraInput;
            _spriteBatch = spriteBatch;
            _spriteRender = new SpriteRender(_spriteBatch);
            _spriteSheetLoader = spriteSheetLoader;            
        }    
        
        public async Task LoadAsync()
        {
            _stickSheet = await _spriteSheetLoader.LoadAsync("StickSheet");

            _standLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.Stick_stand });

            _standRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new String[] { StickSheet.Stick_stand });

            _standUpAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.Stick_stand });

            _standDownAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.Stick_stand });

            _walkLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { StickSheet.Stick_walk_right_01,
                StickSheet.Stick_walk_right_02 });

            _walkRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.None,
                new string[] {  StickSheet.Stick_walk_right_01,
                StickSheet.Stick_walk_right_02 });            

            _jumpRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.Stick_jump_right_01 });

            _jumpLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { StickSheet.Stick_jump_right_01 });
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
            else if (entity.State is WalkingRight)
            {
                _currentAnimation = _walkRightAnimation;
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
                150 /
                Math.Max(1, ((Math.Abs(entity.Velocity.X)) / 150)));
            _currentAnimation.SetTimeBetweenFrames(newFrameTime);
            _currentAnimation.Update(deltaTime);

            Vector2 currentFrameOrigin = _currentAnimation.GetCurrentFrame().SpriteFrame.Origin;
            if (entity.BoundingBoxOrigin == default(Vector2) )
            {
                entity.BoundingBoxOrigin = new Vector2(currentFrameOrigin.X - 5, currentFrameOrigin.Y - 30);
            }
        }

        public void Draw(GameTime deltaTime, Entity entity)
        {
            var currentFrame = _currentAnimation.GetCurrentFrame();
            _spriteRender.Draw(currentFrame.SpriteFrame,
                new Vector2(entity.Position.X, entity.Position.Y),
                .5f,
                Color.White,
                rotation: 0,
                scale: entity.Scale,
                spriteEffects: currentFrame.SpriteEffect);

            if (DebugConstants.ShowBoundingBoxes)
            {
                BoundingBoxHelper.DrawRectangle(entity.BoundingBox, GlobalAssets.BBoxOutline, Color.White, _spriteBatch, false, 1);
            }
        }
    }
}
