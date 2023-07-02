using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace BricksGame
{
    internal class SceneGameplay : Scene
    {

        private Song myMusic;
        private GameManager gameManager;
        public bool GamePaused;
  
        public SceneGameplay(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            LoadGameManager();
            LoadAudio();
            LoadBackgroundImage();
            base.Load();
        }

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
                if (GameKeyboard.IsKeyReleased(Keys.Space))
                {
                    MediaPlayer.Resume(); ;
                    GamePaused = false;
                }

            }
        }
        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White); 
            gameManager.Draw(mainGame._spriteBatch);
            base.Draw(gameTime);  
            DrawPauseMessage();
        }

        public void DrawPauseMessage()
        {
            if (GamePaused)
            {
                mainGame._spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(0, 0, mainGame.Window.ClientBounds.Width, mainGame.Window.ClientBounds.Height), Color.Gray*0.5f);
                mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "PAUSE", new Vector2(mainGame.Window.ClientBounds.Width/2-30, mainGame.Window.ClientBounds.Height/2), Color.White);
                mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "Press [SPACE] to continue or [ESC] to quit", new Vector2(mainGame.Window.ClientBounds.Width / 2-220, mainGame.Window.ClientBounds.Height / 2 + 30), Color.White);
            }
        }

        public override void UnLoad()
        {
            StopAudio();
            base.UnLoad();
        }

        public override void End()
        {

            mainGame.gameState.ChangeScene(GameState.SceneType.GameOver);
        }

        private void LoadGameManager()
        {
            gameManager = new GameManager();
            gameManager.Load();
        }
       private void LoadAudio()
        {
            myMusic = AssetsManager.gamePlayMusic;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(myMusic);
        }

        public void PlayAudio()
        {
            MediaPlayer.Play(myMusic);
        }

        public void StopAudio()
        {
            MediaPlayer.Stop();
        }

        public void PauseAudio()
        {
            MediaPlayer.Pause();
        }


        private void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>("images/map1");
        }
    }
}
