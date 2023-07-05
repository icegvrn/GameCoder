using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BricksGame
{
    /// <summary>
    ///  Ecran de victoire du jeu, apparait quand l'utilisateur a terminé tous les niveaux disponibles. Il s'agit uniquement d'une image et une musique de fond.
    /// </summary>
    public class SceneGameWin : Scene
    {
        Song backgroundMusic;
        public SceneGameWin(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        // Chargement de l'image de fond et de la musique
        public override void Load()
        {
            LoadAudio();
            LoadBackgroundImage();
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font14), "[ESC]", new Vector2(10, 11), Color.White);
            base.Draw(gameTime);
        }

        private void LoadAudio()
        {
            backgroundMusic = ServiceLocator.GetService<IMediaPlayerService>().GetMusic(IMediaPlayerService.Musics.victory);
            ServiceLocator.GetService<IMediaPlayerService>().PlayMusic(backgroundMusic, false);
        }

        public void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot()+"screen_victory");
        }

        public override void UnLoad()
        {
            base.UnLoad();
        }

    }
}
