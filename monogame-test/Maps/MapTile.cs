using Microsoft.Xna.Framework;
using monogame_test.RenderHelpers;

namespace monogame_test.Maps
{
    public struct MapTile
    {
        public Vector2 Position { get; set; }
        private RectangleF _boundingBox;
        public RectangleF BoundingBox
        {
            get { return _boundingBox; }
            set
            {
                _boundingBox = new RectangleF(
                    value.X - BoundingBoxOrigin.X,
                    value.Y - BoundingBoxOrigin.Y,
                    value.Width,
                    value.Height);
            }
        }

        public Vector2 BoundingBoxOrigin { get; set; }
        public char ModelChar { get; set; }
    }
}
