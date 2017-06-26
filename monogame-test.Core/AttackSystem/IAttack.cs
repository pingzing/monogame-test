using Microsoft.Xna.Framework;
using monogame_test.Core.AnimationSystem;
using monogame_test.Core.Entities;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.Core.AttackSystem
{
    public interface IAttack
    {
        Animation Animation { get; set; }
        Rectangle BoundingBox { get; set; }
        Vector2 BoundingBoxOrigin { get; set; }
        Entity Owner { get; set; }
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }

        /// <summary>
        /// TODO: Look into this. Maybe represent it as an in-memory WAV, or something.
        /// Should attacks be responsible for loading their own sounds/animations?
        /// </summary>
       string SoundPath { get; set; }

        Task LoadAsync(SpriteSheet attackSheet);
        void Update(GameTime gameTime, Entity entity);
        void Draw(GameTime gameTime, Entity entity);

    }
}
