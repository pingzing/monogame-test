using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_test.Core.Content;
using monogame_test.Core.DebugHelpers;
using monogame_test.Core.RenderHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace monogame_test.Core.Maps
{
    public abstract class MapBase
    {
        protected SpriteBatch MapBatch;
        protected SpriteRender MapRender;
        protected SpriteSheetLoader MapSheetLoader;
        protected SpriteSheet MapSpriteSheet;

        public virtual int TileHeight { get; protected set; } = 32;
        public virtual int TileWidth { get; protected set; } = 32;
        public abstract int MapHeight { get; }
        public abstract int MapWidth { get; }
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

        public virtual async Task LoadAsync()
        {
            MapSpriteSheet = await MapSheetLoader.LoadAsync(TilesetName);

            DrawnMap = new MapTile[MapGrid.Count, MapGrid[0].Length];

            // These offsets account for the fact that tiles are drawn based
            // on their center, so a 32x32 tile drawn at (0, 0) will have its
            // Center at (0, 0), and its Top Left at (-16, -16) without these
            // adjustments.
            int tileXOffset = TileWidth / 2;
            int tileYOffset = TileWidth / 2;
            for (int rowNum = 0; rowNum < MapGrid.Count; rowNum++)
            {
                for (int colNum = 0; colNum < MapGrid[0].Length; colNum++)
                {
                    Vector2 coords = ModelToWorld(rowNum, colNum);
                    DrawnMap[rowNum, colNum] = new MapTile
                    {
                        BoundingBoxOrigin = MapSpriteSheet.Sprite(CharToTile[MapGrid[rowNum][colNum]]).Origin,
                        BoundingBox = new Rectangle((int)coords.X + tileXOffset, (int)coords.Y + tileYOffset, TileWidth, TileHeight),
                        Position = new Vector2(coords.X + tileXOffset, coords.Y + tileYOffset),
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
                    0.1f,
                    Color.White,
                    0,
                    1,
                    SpriteEffects.None);
            }

            foreach (MapTile tile in DrawnMap)
            {
                if (tile.IsCollidable)
                {
                    if (DebugConstants.ShowBoundingBoxes)
                    {
                        BoundingBoxHelper.DrawRectangle(tile.BoundingBox, GlobalAssets.BBoxOutline, Color.White, MapBatch, false, 1);
                    }

                    if ((tile.IsBeingCollided || tile.IsBeingStoodOn) && DebugConstants.ShowCollisionOverlays)
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
