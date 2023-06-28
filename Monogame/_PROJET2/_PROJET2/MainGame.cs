using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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
            WindowSettingsInitialization();
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            RegisterServices();
            AssetsManager.Load(Content);
            gameState.ChangeScene(GameState.SceneType.Menu);

        }

        protected override void Update(GameTime gameTime)
        {
            RegisterInput();
            RegisterScene();
            gameState.CurrentScene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(34, 34, 34));
            _spriteBatch.Begin();
            gameState.CurrentScene.Draw(gameTime);
           _spriteBatch.End();
            base.Draw(gameTime);
            
        }

        private void WindowSettingsInitialization()
        {
            Window.Title = ("Dice Roll Breakout");
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 832;
            _graphics.ApplyChanges();
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterService(Content);
            ServiceLocator.RegisterService(GraphicsDevice);
           
        }

        private void RegisterInput()
        { 
            ServiceLocator.RegisterService(Mouse.GetState());
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }

        private void RegisterScene()
        {
            ServiceLocator.RegisterService(gameState);
         }
   

    }
}