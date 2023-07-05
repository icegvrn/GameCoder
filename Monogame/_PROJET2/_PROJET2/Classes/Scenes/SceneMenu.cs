using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;

namespace BricksGame
{
    /// <summary>
    /// Contient la scène de démarrage du jeu : démarrer le jeu, créer un niveau et charger un niveau quand un niveau a été créé
    /// </summary>
    public class SceneMenu : Scene
    {

        // Boutons du menu
        private Button bttn_Start;
        private Button bttn_Create;
        private Button bttn_Load;
        private SoundEffect sndButton;
        // Musique
        private Song backgroundMusic;
        
        
        public SceneMenu(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            LoadBackgroundImage();
            LoadAudio();
            LoadStartButton();
            LoadEditorButton();
            LoadLoadButton();
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
           base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font14), "[ESC]", new Vector2(10,11), Color.White);
            mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font10), "to quit", new Vector2(10, 25), Color.White);
            base.Draw(gameTime);    
        }

        private void LoadStartButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button"), mainGame.Content.Load<Texture2D>("button_hover") };
            bttn_Start = new Button(myButtonTextureList);
            bttn_Start.onClick = OnStartGame;
            bttn_Start.onHover = OnHover;
            bttn_Start.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Start.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 - bttn_Start.currentTexture.Height/1.2f);
            gameObjectsList.Add(bttn_Start);

        }
        private void LoadEditorButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button_editor"), mainGame.Content.Load<Texture2D>("button_editor_hover") };
            bttn_Create = new Button(myButtonTextureList);
            bttn_Create.onClick = OnStartEditor;
            bttn_Create.onHover = OnHover;
            bttn_Create.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Create.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 + bttn_Create.currentTexture.Height/3f);
            gameObjectsList.Add(bttn_Create);

        }

        private void LoadLoadButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button_load"), mainGame.Content.Load<Texture2D>("button_load_hover") };
            bttn_Load = new Button(myButtonTextureList);
            bttn_Load.onClick = OnStartSavedLevel;
            bttn_Load.onHover = OnHover;
            bttn_Load.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Load.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 + bttn_Load.currentTexture.Height*1.5f);
            
            string savedLevelsPath = ServiceLocator.GetService<IPathsService>().GetJSONSavedLevelPath();
            if (File.Exists(savedLevelsPath))
            {
                gameObjectsList.Add(bttn_Load);
            }
        }

        // Méthodes appelées au clic sur bouton : indique au GameState que l'utilisateur souhaite charger le JSON des niveaux prédéfinis du jeu
        public void OnStartGame(Button p_Button)
        {
            sndButton.Play();
            ServiceLocator.GetService<GameState>().currentLevelsJSON = ServiceLocator.GetService<IPathsService>().GetJSONGameLevelPath();
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
  
        }

        // Méthodes appelées au clic sur bouton : indique au GameState que l'utilisateur souhaite charger le JSON du niveau sauvegardé du jeu
        public void OnStartSavedLevel(Button p_Button)
        {
            sndButton.Play();
            ServiceLocator.GetService<GameState>().currentLevelsJSON = ServiceLocator.GetService<IPathsService>().GetJSONSavedLevelPath();
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        // Méthodes appelées au clic sur bouton : charge la scène d'édition
        public void OnStartEditor(Button p_Button)
        {
            sndButton.Play();
            mainGame.gameState.ChangeScene(GameState.SceneType.Editor);
        }

        public void OnHover(Button p_Button)
        {
            sndButton.Play();
        }

        private void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "screen_home");
        }

        private void LoadAudio()
        {
            backgroundMusic = ServiceLocator.GetService<IMediaPlayerService>().GetMusic(IMediaPlayerService.Musics.menu);
            ServiceLocator.GetService<IMediaPlayerService>().PlayMusic(backgroundMusic, true);
            sndButton = mainGame.Content.Load<SoundEffect>(ServiceLocator.GetService<IPathsService>().GetSoundsRoot() + "button");
        }

        private void StopAudio()
        {
            ServiceLocator.GetService<IMediaPlayerService>().StopMusic();

        }

        // Appelé lorsqu'une nouvelle scène a été appelée
        public override void UnLoad()
        {
            StopAudio();
            base.UnLoad();
        }
    }
}
