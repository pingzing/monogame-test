using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;

namespace monogame_test.Core.Maps
{
    public class MapManager
    {
        public TestMap CurrentMap { get; private set; }

        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteBatch _spriteBatch;

        public MapManager(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteSheetLoader = spriteSheetLoader;
            _spriteBatch = spriteBatch;
            CurrentMap = new TestMap(_spriteSheetLoader, _spriteBatch);
        }

        public void Load()
        {
            CurrentMap.Load();
        }

        public void ChangeMap(TestMap newMap)
        {
            //CurrentMap.Unload(); //todo: implement
            newMap.Load();
            CurrentMap = newMap;
        }

        public void Unload()
        {
            // TODO: Call Map.Unload().
        }

        public void Update(GameTime gameTime)
        {
            CurrentMap.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            CurrentMap.Draw(gameTime);
        }
    }
}
