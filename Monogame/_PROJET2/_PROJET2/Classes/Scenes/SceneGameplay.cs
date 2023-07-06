using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BricksGame
{
    /// <summary>
    /// Scène contenant tout le gameplay et la partie jeu pur.
    /// </summary>
    internal class SceneGameplay : Scene
    {
        private GameManager gameManager;

        private Song backgroundMusic;
        public bool GamePaused { get; set; }
  
        public SceneGameplay(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        // Chargement du gameManager, chargé de la logique du jeu, et de l'audio.
        public override void Load()
        {
            LoadGameManager();
            LoadAudio();
            base.Load();
        }

        // Si le jeu est indiqué comme Win, alors on change de scène pour afficher l'écran de victoire. Si le jeu est en pause, on vérifie si le joueur fait l'action de reprendre ou non.
        public override void Update(GameTime gameTime)
        {
            if (!GamePaused)
            {
                gameManager.Update(gameTime);
                if (gameManager.IsGameWin)
                {
                    mainGame.gameState.ChangeScene(GameState.SceneType.Win);
                }
                base.Update(gameTime);
            }
            else
            {
                if (ServiceLocator.GetService<IInputService>().OnPauseReleased())
                {
                    MediaPlayer.Resume(); ;
                    GamePaused = false;
                }

            }
        }
        public override void Draw(GameTime gameTime)
        {
            gameManager.Draw(mainGame._spriteBatch);
            base.Draw(gameTime);  
            DrawPauseMessage();
        }

        // Chargement du GameManager, qui gère la logique du jeu. 
        private void LoadGameManager()
        {
            gameManager = new GameManager();
            gameManager.Load();
        }


        // Chargement de la musique de fond
        private void LoadAudio()
        {
            backgroundMusic = ServiceLocator.GetService<IMediaPlayerService>().GetMusic(IMediaPlayerService.Musics.game);
            ServiceLocator.GetService<IMediaPlayerService>().PlayMusic(backgroundMusic, true);
        }


        public void PlayAudio()
        {
            ServiceLocator.GetService<IMediaPlayerService>().PlayMusic(backgroundMusic, true);
        }

        public void StopAudio()
        {
            ServiceLocator.GetService<IMediaPlayerService>().StopMusic();
        }

        public void PauseAudio()
        {
            ServiceLocator.GetService<IMediaPlayerService>().PauseMusic();
        }

        // Dessin du message de pause en cours de jeu
        public void DrawPauseMessage()
        {
            if (GamePaused)
            {
               Texture2D blankTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
                mainGame._spriteBatch.Draw(blankTexture, new Rectangle(0, 0, mainGame.Window.ClientBounds.Width, mainGame.Window.ClientBounds.Height), Color.Gray*0.5f);
                mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), "PAUSE", new Vector2(mainGame.Window.ClientBounds.Width/2-30, mainGame.Window.ClientBounds.Height/2), Color.White);
                mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), "Press [SPACE] to continue or [ESC] to quit", new Vector2(mainGame.Window.ClientBounds.Width / 2-220, mainGame.Window.ClientBounds.Height / 2 + 30), Color.White);
            }
        }

        public override void UnLoad()
        {
            StopAudio();
            base.UnLoad();
        }

        // Déclanché par le GameManager lorsque le joueur a perdu : change de scène pour aller sur le GameOver.
        public override void End()
        {
            mainGame.gameState.ChangeScene(GameState.SceneType.GameOver);
        }
    }
}
