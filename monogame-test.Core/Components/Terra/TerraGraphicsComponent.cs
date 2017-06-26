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

        private Animation _punchRightAnimation;
        private Animation _punchLeftAnimation;

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
                new string[] { StickSheet.StickStand });

            _standRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new String[] { StickSheet.StickStand });

            _standUpAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.StickStand });

            _standDownAnimation = new Animation(
                TimeSpan.FromMilliseconds(300),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.StickStand });

            _walkLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { StickSheet.StickWalkRight01,
                StickSheet.StickWalkRight02,
                StickSheet.StickWalkRight03,
                StickSheet.StickWalkRight02});

            _walkRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.StickWalkRight01,
                StickSheet.StickWalkRight02,
                StickSheet.StickWalkRight03,
                StickSheet.StickWalkRight02});

            _jumpRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.None,
                new string[] { StickSheet.StickRightJump01 });

            _jumpLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(200),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new string[] { StickSheet.StickRightJump01 });

            _punchRightAnimation = new Animation(
                TimeSpan.FromMilliseconds(150),
                _stickSheet,
                SpriteEffects.None,
                new string[] {
                    StickSheet.StickPunch3,
                    StickSheet.StickPunch2,
                    StickSheet.StickPunch1,
                    StickSheet.StickPunch1 });

            _punchLeftAnimation = new Animation(
                TimeSpan.FromMilliseconds(150),
                _stickSheet,
                SpriteEffects.FlipHorizontally,
                new string[] {
                    StickSheet.StickPunch3,
                    StickSheet.StickPunch2,
                    StickSheet.StickPunch1,                                    
                    StickSheet.StickPunch1 });
        }


        public void Update(GameTime deltaTime, Entity entity)
        {                     
            if (entity.State is StandingDown)
            {
                SetCurrentAnimation(_standDownAnimation, entity, deltaTime);
            }
            else if (entity.State is StandingLeft)
            {
                SetCurrentAnimation(_standLeftAnimation, entity, deltaTime);                
            }
            else if (entity.State is StandingRight)
            {
                SetCurrentAnimation(_standRightAnimation, entity, deltaTime);                
            }
            else if (entity.State is StandingUp)
            {
                SetCurrentAnimation(_standUpAnimation, entity, deltaTime);                
            }
            else if (entity.State is WalkingLeft)
            {
                SetCurrentAnimation(_walkLeftAnimation, entity, deltaTime);                
            }            
            else if (entity.State is WalkingRight)
            {
                SetCurrentAnimation(_walkRightAnimation, entity, deltaTime);
            }            
            else if (entity.State is JumpingLeft)
            {
                SetCurrentAnimation(_jumpLeftAnimation, entity, deltaTime);                
            }
            else if (entity.State is JumpingRight)
            {
                SetCurrentAnimation(_jumpRightAnimation, entity, deltaTime);                
            }           
            else if (entity.State is PunchingLeft)
            {
                SetCurrentAnimation(_punchLeftAnimation, entity, deltaTime);                
            }
            else if (entity.State is PunchingRight)
            {
                SetCurrentAnimation(_punchRightAnimation, entity, deltaTime);                
            }                       

            // Set the sprite bounding box origin if it hasn't been set yet
            Vector2 currentFrameOrigin = _currentAnimation.GetCurrentFrame().SpriteFrame.Origin;
            if (entity.BoundingBoxOrigin == default(Vector2))
            {
                // ewwww magic numbers
                entity.BoundingBoxOrigin = new Vector2(currentFrameOrigin.X - 40, currentFrameOrigin.Y);
            }            
        }

        private void SetCurrentAnimation(Animation animation, Entity entity, GameTime deltaTime)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _currentAnimation.Reset();
            }            

            // Adjust time-till-next-frame based on horizontal velocity
            TimeSpan newFrameTime = TimeSpan.FromMilliseconds(
                150 /
                Math.Max(1, ((Math.Abs(entity.Velocity.X)) / 150)));
            _currentAnimation.SetTimeBetweenFrames(newFrameTime);
            _currentAnimation.Update(deltaTime);
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
