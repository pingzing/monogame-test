using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.Core.Maps
{
    public class MapManager
    {
        public MapBase CurrentMap { get; private set; }

        private SpriteSheetLoader _spriteSheetLoader;
        private SpriteBatch _spriteBatch;

        public MapManager(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteSheetLoader = spriteSheetLoader;
            _spriteBatch = spriteBatch;
            CurrentMap = new TestMap(_spriteSheetLoader, _spriteBatch);
        }

        public async Task LoadAsync()
        {
            await CurrentMap.LoadAsync();
        }

        public async Task ChangeMap(TestMap newMap)
        {
            //CurrentMap.Unload(); //todo: implement
            await newMap.LoadAsync();
            CurrentMap = newMap;
        }

        public void Unload()
        {
            // TODO: Call CurrentMap.Unload().
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
