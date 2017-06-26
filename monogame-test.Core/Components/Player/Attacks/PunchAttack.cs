using monogame_test.Core.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using monogame_test.Core.AnimationSystem;
using monogame_test.Core.Entities;
using TexturePackerLoader;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerMonoGameDefinitions;

namespace monogame_test.Core.Components.Player.Attacks
{
    public class PunchAttack : IAttack
    {
        private SpriteSheetLoader _spriteLoader;
        private SpriteBatch _spriteBatch;
        private SpriteSheet _punchAttackSheet;
        private SpriteRender _spriteRender;

        public Animation Animation { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Vector2 BoundingBoxOrigin { get; set; }
        public Entity Owner { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public string SoundPath { get; set; }

        public PunchAttack(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteLoader = spriteSheetLoader;
            _spriteRender = new SpriteRender(_spriteBatch);
        }

        public async Task LoadAsync(SpriteSheet attackSheet)
        {
            _punchAttackSheet = attackSheet;

            Animation = new Animation(
                TimeSpan.FromMilliseconds(100),
                _punchAttackSheet,
                SpriteEffects.None,
                new string[] {
                    EffectsSheet.RoundExplosion01,
                    EffectsSheet.RoundExplosion02,
                    EffectsSheet.RoundExplosion03,
                    EffectsSheet.RoundExplosion04,
                    EffectsSheet.RoundExplosion05 });            

            // shut the compiler up
            await Task.FromResult<bool>(true);
        }

        public void Update(GameTime gameTime, Entity entity)
        {
            SpriteFrame currentFrame = Animation.GetCurrentFrame().SpriteFrame;
            BoundingBox = currentFrame.SourceRectangle;
            BoundingBoxOrigin = currentFrame.Origin;
            Animation.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Entity entity)
        {
            var currentFrame = Animation.GetCurrentFrame();
            _spriteRender.Draw(
                currentFrame.SpriteFrame,
                Position);
        }        
    }
}
