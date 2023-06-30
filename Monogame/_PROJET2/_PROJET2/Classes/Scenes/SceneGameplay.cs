using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace BricksGame
{
    internal class SceneGameplay : Scene
    {

        private Song myMusic;
        private GameManager gameManager;
        private Texture2D grid;
        public SceneGameplay(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            LoadGameManager();
            LoadAudio();
            LoadBackgroundImage();
            grid = mainGame.Content.Load<Texture2D>("images/grid");
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            gameManager.Update(gameTime);
            if (gameManager.IsGameWin)
            {
                mainGame.gameState.ChangeScene(GameState.SceneType.Win);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.Draw(grid, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the Gameplay !", new Vector2(1, 1), Color.White);
            base.Draw(gameTime);
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
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(myMusic);
        }

        private void StopAudio()
        {
            MediaPlayer.Stop();
        }

       private void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>("images/map1_2");
        }
    }
}
