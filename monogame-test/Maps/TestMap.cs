using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.RenderHelpers;
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

        private MapTile[] DrawnMap;

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
            DrawnMap = new MapTile[MapGrid.Count * MapGrid[0].Length];
            int rowNum = 0;
            foreach (var row in MapGrid)
            {
                int colNum = 0;
                foreach (var cell in row)
                {                    
                    Vector2 coords = ModelToWorld(rowNum, colNum);
                    int index = MapGrid[0].Length * rowNum + colNum;
                    DrawnMap[index] = new MapTile
                    {
                        BoundingBoxOrigin = _testMapSheet.Sprite(CharToTile[MapGrid[rowNum][colNum]]).Origin,
                        BoundingBox = new RectangleF(coords.X, coords.Y, TileWidth, TileHeight),
                        Position = new Vector2(coords.X, coords.Y),
                        ModelChar = MapGrid[rowNum][colNum]
                    };

                    colNum++;
                }                
                rowNum++;
            }
        }

        public void Draw(GameTime gameTime)
        {
           foreach (MapTile tile in DrawnMap)
            {
                _spriteRender.Draw(
                    _testMapSheet.Sprite(CharToTile[tile.ModelChar]),                    
                    tile.Position,
                    0.5f,
                    Color.White,
                    0, 
                    1, 
                    SpriteEffects.None);                
            }

           foreach(MapTile tile in DrawnMap)
            {
                if (tile.ModelChar == '_' || tile.ModelChar == '#')
                {
                    BoundingBoxHelper.DrawRectangle(tile.BoundingBox, Game.BBoxOutline, Color.White, _spriteBatch, false, 1);
                }
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

        internal RectangleF GetIntersection(RectangleF proposedNewPosition)
        {
            foreach (MapTile tile in DrawnMap)
            {
                if (tile.ModelChar == '_' || tile.ModelChar == '#')
                {
                    RectangleF intersection = RectangleF.Intersect(proposedNewPosition, tile.BoundingBox);
                    if (intersection != RectangleF.Empty)
                    {
                        return intersection;
                    }
                }
            }
            return RectangleF.Empty;
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
