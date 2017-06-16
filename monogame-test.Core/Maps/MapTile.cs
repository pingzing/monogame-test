using Microsoft.Xna.Framework;
using monogame_test.Core.RenderHelpers;

namespace monogame_test.Core.Maps
{
    public class MapTile
    {
        public Vector2 Position { get; set; }
        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
            set
            {
                _boundingBox = new Rectangle(
                    (int)(value.X - BoundingBoxOrigin.X),
                    (int)(value.Y - BoundingBoxOrigin.Y),
                    value.Width,
                    value.Height);
            }
        }

        public Vector2 BoundingBoxOrigin { get; set; }
        public char ModelChar { get; set; }
        public bool IsCollidable { get; set; }
        public bool IsBeingCollided { get; set; }
        public bool IsBeingStoodOn { get; set; }
    }
}
