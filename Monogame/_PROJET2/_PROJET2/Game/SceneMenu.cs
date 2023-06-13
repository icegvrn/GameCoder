using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal class SceneMenu : Scene
    {

        private Button myButton;
        private Song myMusic;
        private SoundEffect sndExplode;
        public SceneMenu(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public void onHover(Button p_Button)
        {
            sndExplode.Play();
    }
        public void onClickPlay(Button p_Button)
        {
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        public override void Load()
        {
            Rectangle screen = mainGame.Window.ClientBounds;
            List<Texture2D> myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button"));
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button_hover"));
            myButton = new Button(myButtonTextureList);
            myButton.Position = new Vector2(screen.Width/2 - myButton.currentTexture.Width / 2, screen.Height/2 - myButton.currentTexture.Height/2);
            myButton.onClick = onClickPlay;
            myButton.onHover = onHover;
            gameObjects.Add(myButton);
            myMusic = AssetsManager.menuMusic;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(myMusic);
            sndExplode = mainGame.Content.Load<SoundEffect>("explode");
            base.Load();
        }

        public override void UnLoad()
        {
            MediaPlayer.Stop();
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {

      
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the menu!", new Vector2(1,1), Color.White);
            base.Draw(gameTime);
            
        }
    }
}
