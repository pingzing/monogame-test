using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Content;
using monogame_test.Core.RenderHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TexturePackerLoader;

namespace monogame_test.Core.Maps
{
    public abstract class MapBase
    {
        protected SpriteBatch MapBatch;
        protected SpriteRender MapRender;
        protected SpriteSheetLoader MapSheetLoader;
        protected SpriteSheet MapSpriteSheet;

        protected virtual float TileHeight { get; set; } = 32;
        protected virtual float TileWidth { get; set; } = 32;
        protected abstract List<char[]> MapGrid { get; set; }
        protected abstract MapTile[,] DrawnMap { get; set; }
        protected abstract Dictionary<char, string> CharToTile { get; set; }
        protected abstract List<char> CollisionTiles { get; set; }
        protected abstract string TilesetName { get; set; }

        public MapBase(SpriteSheetLoader loader, SpriteBatch spriteBatch)
        {
            MapSheetLoader = loader;
            MapBatch = spriteBatch;
            MapRender = new SpriteRender(MapBatch);            
        }

        // todo: allow passing in more than a single point, and checking more than a single row simultaneously
        public virtual Rectangle GetNearestHorizontalCollidedObject(float bboxXCoordinate, float bboxMiddleY, bool goingRight)
        {
            if (goingRight)
            {
                Rectangle nearestBoundingBox = default(Rectangle);
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

                Rectangle nearestBoundingBox = default(Rectangle);
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
        public virtual Rectangle GetNearestVerticalCollidedObject(float bboxYCoordinate, float bboxMiddleX, bool goingDown)
        {
            if (goingDown)
            {
                Rectangle nearestBoundingBox = default(Rectangle);
                float bboxDiff = float.MaxValue;

                int columnIndex = (int)bboxMiddleX / (int)TileWidth;
                int endIndex = MapGrid.Count;
                for (int i = 0; i < endIndex; i++)
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
                Rectangle nearestBoundingBox = default(Rectangle);
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

        public virtual void Load()
        {
            MapSpriteSheet = MapSheetLoader.Load(TilesetName);

            DrawnMap = new MapTile[MapGrid.Count, MapGrid[0].Length];

            for (int rowNum = 0; rowNum < MapGrid.Count; rowNum++)
            {
                for (int colNum = 0; colNum < MapGrid[0].Length; colNum++)
                {
                    Vector2 coords = ModelToWorld(rowNum, colNum);
                    DrawnMap[rowNum, colNum] = new MapTile
                    {
                        BoundingBoxOrigin = MapSpriteSheet.Sprite(CharToTile[MapGrid[rowNum][colNum]]).Origin,
                        BoundingBox = new RectangleF(coords.X, coords.Y, TileWidth, TileHeight),
                        Position = new Vector2(coords.X, coords.Y),
                        ModelChar = MapGrid[rowNum][colNum],
                        IsCollidable = CollisionTiles.Contains(MapGrid[rowNum][colNum])
                    };
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (MapTile tile in DrawnMap)
            {
                MapRender.Draw(
                    MapSpriteSheet.Sprite(CharToTile[tile.ModelChar]),
                    tile.Position,
                    0.5f,
                    Color.White,
                    0,
                    1,
                    SpriteEffects.None);
            }

            foreach (MapTile tile in DrawnMap)
            {
                if (tile.IsCollidable)
                {
                    BoundingBoxHelper.DrawRectangle(tile.BoundingBox, GlobalAssets.BBoxOutline, Color.White, MapBatch, false, 1);
                    if (tile.IsBeingCollided || tile.IsBeingStoodOn)
                    {
                        BoundingBoxHelper.DrawRectangle(tile.BoundingBox, GlobalAssets.CollisionOverlay, Color.White, MapBatch, true, 1);
                    }
                }
            }
        }

        public virtual bool IsColliding(Rectangle proposedNewPosition)
        {
            foreach (MapTile tile in DrawnMap)
            {
                if (tile.IsCollidable)
                {
                    Rectangle intersection = Rectangle.Intersect(proposedNewPosition, tile.BoundingBox);
                    if (intersection != Rectangle.Empty)
                    {
                        tile.IsBeingCollided = true;
                        return true;
                    }
                    else
                    {
                        tile.IsBeingCollided = false;
                    }
                }
            }
            return false;
        }

        // TODO: Fix this to actually check the entire bottom chunk of the bounding box, and not just the center.
        public virtual bool IsStandingOnGround(Rectangle boundingBox)
        {
            bool isStoodUpon = false;
            foreach (MapTile tile in DrawnMap)
            {
                if (tile.IsCollidable)
                {
                    if ((boundingBox.Center.X >= tile.BoundingBox.Left && boundingBox.Center.X <= tile.BoundingBox.Right)
                        && Math.Abs(boundingBox.Bottom - tile.BoundingBox.Top) <= 2)
                    {
                        tile.IsBeingStoodOn = true;
                        isStoodUpon = true;
                    }
                    else
                    {
                        tile.IsBeingStoodOn = false;
                    }
                }
            }

            return isStoodUpon;
        }

        protected Vector2 ModelToWorld(int rowNum, int colNum)
        {
            return new Vector2((colNum) * TileWidth, (rowNum) * TileHeight);
        }

        protected Point WorldToModelIndex(Vector2 position)
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
