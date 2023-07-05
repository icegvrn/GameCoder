using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    public class MainGame : Game
    {
        // Gestion graphique
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

       // Services utilisés dans le jeu
        private InputServices _inputServices;
        private FontService _fontService;
        private AssetsManagerService _assetsManagerService;
        private PathsService _pathsService;
        private PlayerSessionService _playerSessionService;
        private MediaPlayerService _mediaPlayerService;

        // GameState pour gérer les scènes
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
            LoadServices();
            RegisterServices();

            // Le jeu démarre sur le menu
            gameState.ChangeScene(GameState.SceneType.Menu);
        }

        public void LoadServices()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fontService = new FontService(Content);
            _assetsManagerService = new AssetsManagerService(Content);
            _inputServices = new InputServices();
            _pathsService = new PathsService();
            _playerSessionService = new PlayerSessionService();
            _mediaPlayerService = new MediaPlayerService();
        }

        protected override void Update(GameTime gameTime)
        {
            // Mise à jour des input utilisateur
            _inputServices.InputUpdateBegin();
            RegisterInput();
            RegisterScene();

            // Mise à jour de la scène courante 
            gameState.CurrentScene.Update(gameTime);
            base.Update(gameTime);

            // Enregistrement des dernières actions utilisateur
            _inputServices.InputUpdateEnd();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Nettoyage de la frame précédente
            GraphicsDevice.Clear(new Color(34, 34, 34));

            // Démarrage de la pile de Draw et Draw de la scène courante
            _spriteBatch.Begin();
            gameState.CurrentScene.Draw(gameTime);
           _spriteBatch.End();
            base.Draw(gameTime);
            
        }

        // Paramètres fenêtre
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
            ServiceLocator.RegisterService<IFontService>(_fontService);
            ServiceLocator.RegisterService<IAssetsServices>(_assetsManagerService);
            ServiceLocator.RegisterService<IInputService>(_inputServices);
            ServiceLocator.RegisterService<IPathsService>(_pathsService);
            ServiceLocator.RegisterService<ISessionService>(_playerSessionService);
            ServiceLocator.RegisterService<IMediaPlayerService>(_mediaPlayerService);

        }

        // Enregistrement des touches globales à tout le jeu 
        private void RegisterInput()
        { 
            if (ServiceLocator.GetService<IInputService>().OnReturnReleased())
            {
                if (gameState.CurrentScene is SceneMenu)
                {
                    Exit();
                }
                else
                {
                    if (gameState.CurrentScene is SceneGameplay && !((SceneGameplay)gameState.CurrentScene).GamePaused)
                    {
                        ((SceneGameplay)gameState.CurrentScene).GamePaused = true;
                        ((SceneGameplay)gameState.CurrentScene).PauseAudio();
                    }
                    else
                    {
                        gameState.ChangeScene(GameState.SceneType.Menu);
                    } 
                }
            }
        }

        // Enregistrement de la GameState pour pouvoir accéder à la scène en cours tout le temps
        private void RegisterScene()
        {
            ServiceLocator.RegisterService(gameState);
        }
   
    }
}