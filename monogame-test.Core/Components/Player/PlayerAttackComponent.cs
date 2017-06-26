using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using monogame_test.Core.AttackSystem;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;
using monogame_test.Core.Components.Player.Attacks;
using monogame_test.Core.Components.Player.States;

namespace monogame_test.Core.Components.Player
{
    public class PlayerAttackComponent : IDrawableComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteSheetLoader _spriteLoader;
        private SpriteSheet _effectsSheet;

        private IAttack _punchAttack;
        private IAttack _currentAttack;

        public PlayerAttackComponent(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteLoader = spriteSheetLoader;
            _punchAttack = new PunchAttack(_spriteLoader, _spriteBatch);
        }

        public async Task LoadAsync()
        {
            _effectsSheet = await _spriteLoader.LoadAsync("EffectsSheet");
            await _punchAttack.LoadAsync(_effectsSheet);
        }

        public void Update(GameTime deltaTime, Entity entity)
        {
            // update current attack
            if (entity.State is Punching)
            {
                bool isNewState = _currentAttack != _punchAttack;
                _currentAttack = _punchAttack;
                _currentAttack.Owner = entity;
                _currentAttack.Position = new Vector2(entity.Position.X + 50, entity.Position.Y);
                if (isNewState)
                {
                    _currentAttack.Animation.Reset();
                }
            }
            else
            {
                _currentAttack = null;
            }

            if (_currentAttack != null)
            {
                _currentAttack.Update(deltaTime, entity);
            }
        }

        public void Draw(GameTime gameTime, Entity entity)
        {
            if (_currentAttack != null)
            {
                _currentAttack.Draw(gameTime, entity);
            }
        }
    }
}
