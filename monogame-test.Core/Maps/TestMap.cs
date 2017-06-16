using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace monogame_test.Core.Maps
{
    public class TestMap : MapBase
    {
        protected override float TileHeight { get; set; } = 32;
        protected override float TileWidth { get; set; } = 32;

        protected override List<char[]> MapGrid { get; set; } = new List<char[]> {
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..........................__..".ToCharArray(),
            "..............................".ToCharArray(),
            ".................______.......".ToCharArray(),
            "..............................".ToCharArray(),
            "........_______...............".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "##############################".ToCharArray(),
        };

        protected override MapTile[,] DrawnMap { get; set; }

        protected override Dictionary<char, string> CharToTile { get; set; } = new Dictionary<char, string>
        {
            { '.', CafeTileset.CarpetMiddle},
            { '_', CafeTileset.BrickGrey },
            { '#', CafeTileset.BrickBlock }
        };

        protected override List<char> CollisionTiles { get; set; } = new List<char> { '_', '#' };

        protected override string TilesetName { get; set; } = "CafeTileset";

        public TestMap(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch) : base(spriteSheetLoader, spriteBatch) { }
    }        
}
