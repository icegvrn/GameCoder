using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BricksGame
{
    internal class SceneGameWin : Scene
    {
        Song backgroundMusic;
        public SceneGameWin(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            LoadAudio();
            LoadBackgroundImage();
            base.Load();
        }

        private void LoadAudio()
        {
            backgroundMusic = AssetsManager.victoryPlayMusic;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(backgroundMusic);
        }

        public override void UnLoad()
        {
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(AssetsManager.Font14, "[ESC]", new Vector2(10, 11), Color.White);
            base.Draw(gameTime);
        }

        public void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>("images/screen_victory");
        }
    }
}
