using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame_test.Core.CameraSystem;
using monogame_test.Core.Content;
using monogame_test.Core.Entities;
using monogame_test.Core.Maps;
using TexturePackerLoader;

namespace monogame_test.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteSheetLoader _spriteSheetLoader;
        private EntityFactory _factory;
        private Camera _camera;
        private MapManager _mapManager;        

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 944;
            _graphics.PreferredBackBufferHeight = 464;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _camera = new Camera();
            _camera.ViewportWidth = _graphics.GraphicsDevice.Viewport.Width;
            _camera.ViewportHeight = _graphics.GraphicsDevice.Viewport.Height;

            DebugHelpers.DebugConstants.ShowBoundingBoxes = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GlobalAssets.Load(Content);
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteSheetLoader = new SpriteSheetLoader(this.Content);            

            _mapManager = new MapManager(_spriteSheetLoader, _spriteBatch);
            _mapManager.Load();

            _factory = new EntityFactory(_graphics.GraphicsDevice, _spriteSheetLoader, _spriteBatch, _mapManager);
            _factory.CreateTerraEntity();

            // TODO: use this.Content to load your game content here                                    
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _mapManager.Update(gameTime);

            foreach (Entity entity in _factory.EntityRegistry)
            {
                entity.Update(gameTime);
            }
            _camera.Update(_factory.PlayerEntity, _mapManager.CurrentMap);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, 
                samplerState: SamplerState.PointClamp, 
                transformMatrix: _camera.TranslationMatrix);

            _mapManager.Draw(gameTime);

            foreach (Entity entity in _factory.EntityRegistry)
            {
                entity.Draw(gameTime);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
