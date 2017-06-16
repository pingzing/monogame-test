using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.RenderHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace monogame_test.Core.Maps
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
            "..........................__..".ToCharArray(),
            "..............................".ToCharArray(),
            ".................______.......".ToCharArray(),
            "..............................".ToCharArray(),
            "........_______...............".ToCharArray(),
            "..............................".ToCharArray(),
            "..............................".ToCharArray(),
            "##############################".ToCharArray(),
        };

        private MapTile[,] DrawnMap;

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

        // todo: allow passing in more than a single point, and checking more than a single row simultaneously
        internal RectangleF GetNearestHorizontalCollidedObject(float bboxXCoordinate, float bboxMiddleY, bool goingRight)
        {
            if (goingRight)
            {
                RectangleF nearestBoundingBox = default(RectangleF);
                float bboxDiff = float.MaxValue;

                int rowIndex = (int)bboxMiddleY / (int)TileHeight;
                int endIndex = MapGrid[0].Length;
                for (int i = 0; i < endIndex; i++)
                {
                    if (Math.Abs(DrawnMap[rowIndex, i].BoundingBox.Left - bboxXCoordinate) < bboxDiff)
                    {
                        nearestBoundingBox = DrawnMap[rowIndex, i].BoundingBox;
                        bboxDiff = Math.Abs(DrawnMap[rowIndex, i].BoundingBox.Left - bboxXCoordinate);
                    }
                }
                return nearestBoundingBox;
            }
            else
            {
                int rowIndex = (int)bboxMiddleY / (int)TileHeight;
                int startIndex = MapGrid[0].Length - 1;

                RectangleF nearestBoundingBox = default(RectangleF);
                float bboxDiff = float.MaxValue;
                for (int i = startIndex; i >= 0; i--)
                {
                    if (Math.Abs(DrawnMap[rowIndex, i].BoundingBox.Right - bboxXCoordinate) < bboxDiff)
                    {
                        nearestBoundingBox = DrawnMap[rowIndex, i].BoundingBox;
                        bboxDiff = Math.Abs(DrawnMap[rowIndex, i].BoundingBox.Right - bboxXCoordinate);
                    }
                }
                return nearestBoundingBox;
            }
        }

        // todo: allow passing in more than a single point, and checking more than a single column simultaneously
        internal RectangleF GetNearestVerticalCollidedObject(float bboxYCoordinate, float bboxMiddleX, bool goingDown)
        {
            if (goingDown)
            {
                RectangleF nearestBoundingBox = default(RectangleF);
                float bboxDiff = float.MaxValue;

                int columnIndex = (int)bboxMiddleX / (int)TileWidth;
                int endIndex = MapGrid.Count;
                for(int i = 0; i < endIndex; i++)
                {
                    if (Math.Abs(DrawnMap[i, columnIndex].BoundingBox.Top - bboxYCoordinate) < bboxDiff)
                    {
                        nearestBoundingBox = DrawnMap[i, columnIndex].BoundingBox;
                        bboxDiff = Math.Abs(DrawnMap[i, columnIndex].BoundingBox.Top - bboxYCoordinate);
                    }                    
                }
                return nearestBoundingBox;
            }
            else
            {
                RectangleF nearestBoundingBox = default(RectangleF);
                float bboxDiff = float.MaxValue;

                int columnIndex = (int)bboxYCoordinate / (int)TileWidth;
                int startIndex = MapGrid.Count - 1;
                for (int i = startIndex; i >= 0; i--)
                {
                    if (Math.Abs(DrawnMap[i, columnIndex].BoundingBox.Bottom - bboxYCoordinate) < bboxDiff)
                    {
                        nearestBoundingBox = DrawnMap[i, columnIndex].BoundingBox;
                        bboxDiff = Math.Abs(DrawnMap[i, columnIndex].BoundingBox.Bottom - bboxYCoordinate);
                    }
                }
                return nearestBoundingBox;
            }
        }

        public void Load()
        {
            _testMapSheet = _spriteLoader.Load(TilesetName);
        }

        public void Update(GameTime gameTime)
        {            
            DrawnMap = new MapTile[MapGrid.Count, MapGrid[0].Length];
            
            for(int rowNum = 0; rowNum < MapGrid.Count; rowNum++)            
            {                
                for (int colNum = 0; colNum < MapGrid[0].Length; colNum++)
                {                    
                    Vector2 coords = ModelToWorld(rowNum, colNum);                    
                    DrawnMap[rowNum, colNum] = new MapTile
                    {
                        BoundingBoxOrigin = _testMapSheet.Sprite(CharToTile[MapGrid[rowNum][colNum]]).Origin,
                        BoundingBox = new RectangleF(coords.X, coords.Y, TileWidth, TileHeight),
                        Position = new Vector2(coords.X, coords.Y),
                        ModelChar = MapGrid[rowNum][colNum]
                    };                    
                }                
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
                    //BoundingBoxHelper.DrawRectangle(tile.BoundingBox, Game.BBoxOutline, Color.White, _spriteBatch, false, 1);
                }
            }
        }       

        internal bool IsColliding(RectangleF proposedNewPosition)
        {
            foreach (MapTile tile in DrawnMap)
            {
                if (tile.ModelChar == '_' || tile.ModelChar == '#')
                {
                    RectangleF intersection = RectangleF.Intersect(proposedNewPosition, tile.BoundingBox);
                    if (intersection != RectangleF.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        // TODO: Fix this to actually check the entire bottom chunk of the bounding box, and not just the center.
        internal bool IsStandingOnGround(RectangleF boundingBox)
        {
            foreach (MapTile tile in DrawnMap)
            {
                if (tile.ModelChar == '_' || tile.ModelChar == '#')
                {
                    if ((boundingBox.Center.X >= tile.BoundingBox.Left && boundingBox.Center.X <= tile.BoundingBox.Right)
                        && Math.Abs(boundingBox.Bottom - tile.BoundingBox.Top) <= 2 )
                    {
                        return true;
                    }
                }
            }
            return false;
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
