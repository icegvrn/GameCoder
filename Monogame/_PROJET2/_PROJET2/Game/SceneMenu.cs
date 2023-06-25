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
        private Button bttn_Start;
        private Button bttn_Create;
        private Song myMusic;
        private SoundEffect sndExplode;
        public SceneMenu(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public void onHover(Button p_Button)
        {
            sndExplode.Play();
    }
        public void StartGame(Button p_Button)
        {
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        public void StartEditor(Button p_Button)
        {
            mainGame.gameState.ChangeScene(GameState.SceneType.Editor);
        }

        public override void Load()
        {
            Rectangle screen = mainGame.Window.ClientBounds;
            List<Texture2D> myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button"));
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button_hover"));
            bttn_Start = new Button(myButtonTextureList);
            bttn_Start.Position = new Vector2(screen.Width/2 - bttn_Start.currentTexture.Width / 2, screen.Height/2 - bttn_Start.currentTexture.Height);
            bttn_Start.onClick = StartGame;
            bttn_Start.onHover = onHover;
            gameObjects.Add(bttn_Start);

            myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button_editor"));
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("button_editor_hover"));
            bttn_Start = new Button(myButtonTextureList);
            bttn_Create = new Button(myButtonTextureList);
            bttn_Create.Position = new Vector2(screen.Width / 2 - bttn_Create.currentTexture.Width / 2, screen.Height / 2 + bttn_Create.currentTexture.Height);
            bttn_Create.onClick = StartEditor;
            bttn_Create.onHover = onHover;
            gameObjects.Add(bttn_Create);

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
