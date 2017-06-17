using Microsoft.Xna.Framework;
using monogame_test.Core.Entities;
using monogame_test.Core.Maps;

namespace monogame_test.Core.CameraSystem
{
    public class Camera
    {
        /// <summary>
        /// Construct a new Camera class with standard zoom (no scaling)
        /// </summary>
        public Camera()
        {
            Zoom = 1f;
        }

        /// <summary>
        /// Construct a new camera with the specified zoom level.
        /// </summary>
        /// <param name="startingZoom">The level of zoom, between 0 and n.</param>
        public Camera(float startingZoom)
        {
            Zoom = startingZoom;
        }

        /// <summary>
        /// Centered Position of the Camera in pixels.
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// Current Zoom level with 1.0f being standard
        /// </summary>
        public float Zoom { get; private set; }
        /// <summary>
        /// Current Rotation amount with 0.0f being standard orientation
        /// </summary>
        public float Rotation { get; private set; }

        /// <summary>
        /// Height of the viewport window which we need to adjust any time the player resizes the game window.
        /// </summary>
        public int ViewportWidth { get; set; }
        /// <summary>
        /// Width of the viewport window which we need to adjust any time the player resizes the game window.
        /// </summary>
        public int ViewportHeight { get; set; }

        /// <summary>
        /// Center of the Viewport which does not account for scale
        /// </summary>
        public Vector2 ViewportCenter => new Vector2(ViewportWidth* 0.5f, ViewportHeight* 0.5f);

        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                -(int)Position.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        /// <summary>
        /// Call this method with negative values to zoom out or positive values to zoom in. 
        /// It looks at the current zoom and adjusts it by the specified amount. If we were 
        /// at a 1.0f zoom level and specified -0.5f amount it would leave us with 1.0f - 
        /// 0.5f = 0.5f so everything would be drawn at half size.
        /// </summary>
        /// <param name="amount">Amount to zoom in or out by. Accepts negative values.</param>
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        /// <summary>
        ///  Move the camera in an X and Y amount based on the cameraMovement param.
        /// if clampToMap is true the camera will try not to pan outside of the
        /// bounds of the map.
        /// </summary>
        /// <param name="cameraMovement">Moves the camera by X and Y amount.</param>
        /// <param name="clampToMap">Set to true to disallow the camera from leaving the bounds of the map.</param>        
        public void MoveCamera(Vector2 cameraMovement, MapBase map, bool clampToMap = false)
        {
            Vector2 newPosition = Position + cameraMovement;

            if (clampToMap)
            {
                Position = GetClampedPosition(newPosition, map);
            }
            else
            {
                Position = newPosition;
            }
        }

        public Rectangle GetViewportWorldBounds()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner =
            ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X,
            (int)viewPortCorner.Y,
            (int)(viewPortBottomCorner.X - viewPortCorner.X),
            (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
        }

        /// <summary>
        /// Center the camera on specific Entity.
        /// </summary>
        /// <param name="entity">The entity to center on.</param>
        /// <param name="map">The map hosting the camera.</param>
        public void CenterOn(Entity entity, MapBase map)
        {
            Position = GetCenteredPosition(entity, map, true);
        }

        private Vector2 GetCenteredPosition(Entity entity, MapBase map, bool clampToMap = false)
        {
            var cameraPosition = new Vector2(
                entity.Position.X * entity.BoundingBox.Width, 
                entity.Position.Y * entity.BoundingBox.Height);
            var cameraCenteredOnTilePosition = new Vector2(
                cameraPosition.X + entity.BoundingBox.Width,
                cameraPosition.Y + entity.BoundingBox.Height);

            if (clampToMap)
            {
                return GetClampedPosition(cameraCenteredOnTilePosition, map);
            }

            return cameraCenteredOnTilePosition;
        }

        /// <summary>
        /// Center the camera on specific pixel coordinates.
        /// </summary>
        /// <param name="coords"></param>
        public void CenterOn(Vector2 coords, MapBase map)
        {
            Position = GetCenteredPosition(coords, map, true);
        }

        private Vector2 GetCenteredPosition(Vector2 coords, MapBase map, bool clampToMap = false)
        {
            var cameraPosition = new Vector2(coords.X, coords.Y);
            var cameraCenteredOnTilePosition = new Vector2(
                cameraPosition.X,
                cameraPosition.Y);
            if (clampToMap)
            {
                return GetClampedPosition(cameraCenteredOnTilePosition, map);
            }

            return cameraCenteredOnTilePosition;
        }

        // Clamp the camera so it never leaves the visible area of the map.
        private Vector2 GetClampedPosition(Vector2 position, MapBase map)
        {
            var cameraMax = new Vector2(
                map.MapWidth - (ViewportWidth / Zoom / 2),
                map.MapHeight - (ViewportHeight / Zoom / 2));

            return Vector2.Clamp(position,
                new Vector2(ViewportWidth / Zoom / 2, 
                ViewportHeight / Zoom / 2),
                cameraMax);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition,
            Matrix.Invert(TranslationMatrix));
        }

        // Move the camera's position based on input
        public void Update(Entity player, MapBase map)
        {
            Vector2 cameraMovement = Vector2.Zero;
            cameraMovement = player.Position - Position;            

            // Move the camera at one-eighth of our target movement per frame to add some floatiness
            cameraMovement = cameraMovement / 8;
            MoveCamera(cameraMovement, map, true);
        }
    }
}
