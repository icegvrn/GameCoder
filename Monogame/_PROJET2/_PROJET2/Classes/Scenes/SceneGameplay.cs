using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal class SceneGameplay : Scene
    {

        private Song myMusic;
        private GameManager gameManager;
        private Texture2D background;
        private Texture2D grid;
        public SceneGameplay(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            gameManager = new GameManager(this);
            Rectangle Screen = mainGame.Window.ClientBounds;

            myMusic = AssetsManager.gamePlayMusic;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(myMusic);
            gameManager.Load();
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            background = content.Load<Texture2D>("images/map1");
            grid = content.Load<Texture2D>("images/grid");
            base.Load();
        }

        public override void UnLoad()
        {
            MediaPlayer.Stop();
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {
            gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void End()
        {
            mainGame.gameState.ChangeScene(GameState.SceneType.GameOver);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.Draw(grid, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the Gameplay !", new Vector2(1, 1), Color.White);
            base.Draw(gameTime);
        }
    }
}
