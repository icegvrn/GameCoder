using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace BricksGame
{
    public class MainGame : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public GameState gameState;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            gameState = new GameState(this);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 820;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ServiceLocator.RegisterService(this.Content);
            ServiceLocator.RegisterService(this.GraphicsDevice);
          

            AssetsManager.Load(this.Content);
            gameState.ChangeScene(GameState.SceneType.Menu);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            ServiceLocator.RegisterService(Mouse.GetState());
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            gameState.CurrentScene.Update(gameTime);

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(34, 34, 34));
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            gameState.CurrentScene.Draw(gameTime);
           _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}