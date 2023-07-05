using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BricksGame
{
    /// <summary>
    ///  Ecran de défaite du jeu, apparait quand l'utilisateur a perdu toute sa vie ou n'a plus de balle. Il s'agit uniquement d'une image et une musique de fond.
    /// </summary>
    public class SceneGameOver : Scene
    {
        Song backgroundMusic;

        public SceneGameOver(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        // Chargement de l'image de fond et de la musique
        public override void Load()
        {
            LoadAudio();
            LoadBackgroundImage();
            base.Load();
        }

        // Chargement de la musique de fond
        private void LoadAudio()
        {
            backgroundMusic = ServiceLocator.GetService<IMediaPlayerService>().GetMusic(IMediaPlayerService.Musics.gameOver);
            ServiceLocator.GetService<IMediaPlayerService>().PlayMusic(backgroundMusic, false);
        }

        // Définition d'une touche permettant de recommencer directement, en plus de la touche globale habituelle permettant de quitter.
        public override void Update(GameTime gameTime)
        {
            if (ServiceLocator.GetService<IInputService>().OnPauseReleased())
            {
                ServiceLocator.GetService<GameState>().ChangeScene(GameState.SceneType.Gameplay);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font14), "[SPACE] to retry !", new Vector2(10, 11), Color.White);
            base.Draw(gameTime);
        }

        public void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "screen_gameover");
        }

        public override void UnLoad()
        {
            base.UnLoad();
        }
    }
}
