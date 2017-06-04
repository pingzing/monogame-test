using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace monogame_test.Maps
{
    public class TestMap
    {
        private const float TileHeight = 32;
        private const float TileWidth = 32;        

        public List<char[]> MapGrid = new List<char[]> {
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            ".............______.......__..".ToCharArray(),
            "..............................".ToCharArray(),
            "........_______...............".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "##############################".ToCharArray(),
        };

        private readonly Dictionary<char, string> CharToTile = new Dictionary<char, string>
        {
            { '.', CafeTileset.CarpetMiddle},
            { '_', CafeTileset.BrickGrey },
            { '#', CafeTileset.BrickBlock }
        };

        private const string TilesetName = "CafeTileset";

        private SpriteBatch _spriteBatch;
        private SpriteRender _spriteRender;
        private SpriteSheetLoader _spriteLoader;
        private SpriteSheet _testMapSheet;


        public TestMap(SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteRender = new SpriteRender(_spriteBatch);
            _spriteLoader = spriteSheetLoader;
        }

        public void Load()
        {
            _testMapSheet = _spriteLoader.Load(TilesetName);
        }

        public void Update(GameTime gameTime)
        {
            // ???
        }

        public void Draw(GameTime gameTime)
        {
            int rowNum = 0;
            foreach (var row in MapGrid)
            {
                int colNum = 0;
                foreach (var cell in row)
                {
                    _spriteRender.Draw(
                        sprite: _testMapSheet.Sprite(CharToTile[cell]),
                        position: ModelToWorld(rowNum, colNum),
                        layerDepth: 0,
                        color: Color.White,
                        rotation: 0,
                        scale: 1,
                        spriteEffects: SpriteEffects.None);
                    colNum++;
                }
                rowNum++;
            }
        }

        internal Vector2 GetNearestEdge(Vector2 proposedNewPosition)
        {
            // cheapass algorithm for now: just keep going left until we find an open space big enough
            Point index = WorldToModelIndex(proposedNewPosition);
            int row = index.Y;
            int col = index.X;
            char cell = ' ';
            do
            {                
                cell = MapGrid[row][col];
                col--;
            } while (cell != '#' && cell != '_');

            return ModelToWorld(row, col);
        }

        internal Rectangle GetIntersection(Rectangle proposedNewPosition)
        {
            int rowNum = 0;
            foreach (var row in MapGrid)
            {
                int colNum = 0;
                foreach (var cell in row)
                {
                    if (cell == '_' || cell == '#')
                    {
                        var blockCoords = ModelToWorld(rowNum, colNum);
                        var rectangle = new Rectangle((int)blockCoords.X, (int)blockCoords.Y, (int)TileWidth, (int)TileHeight);
                        Rectangle intersection = Rectangle.Intersect(rectangle, proposedNewPosition);
                        if (!intersection.IsEmpty)
                        {
                            return intersection;
                        }
                    }
                    else
                    {
                        colNum++;
                        continue;
                    }
                    colNum++;
                }
                rowNum++;
            }
            return Rectangle.Empty;
        }

        private Vector2 ModelToWorld(int rowNum, int colNum)
        {
            return new Vector2((colNum) * TileWidth, (rowNum) * TileHeight);
        }

        private Point WorldToModelIndex(Vector2 position)
        {
            int column = (int)position.X / (int)TileWidth;
            int row = (int)position.Y / (int)TileHeight;
            if (row >= MapGrid.Count)
            {
                row = MapGrid.Count - 1;
            }
            if (column >= MapGrid[row].Count())
            {
                column = MapGrid[row].Count() - 1;
            }

            return new Point(column, row);
        }
    }
}
