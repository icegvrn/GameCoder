using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BricksGame
{
    internal class SceneMenu : Scene
    {
        private Button bttn_Start;
        private Button bttn_Create;
        private Button bttn_Load;
        private Song myMusic;
        private SoundEffect sndButton;
        
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

        public override void UnLoad()
        {
            StopAudio();
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {
           base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.White);
            mainGame._spriteBatch.DrawString(AssetsManager.Font14, "[ESC]", new Vector2(10,11), Color.White);
            mainGame._spriteBatch.DrawString(AssetsManager.Font10, "to quit", new Vector2(10, 25), Color.White);
            base.Draw(gameTime);    
        }

        private void LoadBackgroundImage()
        {
            background = mainGame.Content.Load<Texture2D>("images/screen_home");
        }

        private void LoadAudio()
        {
            myMusic = AssetsManager.menuMusic;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(myMusic);
            sndButton = mainGame.Content.Load<SoundEffect>("Sounds/button");
        }

        private void StopAudio()
        {
            MediaPlayer.Stop();
        }

        private void LoadStartButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button"), mainGame.Content.Load<Texture2D>("button_hover") };
            bttn_Start = new Button(myButtonTextureList);
            bttn_Start.onClick = StartGame;
            bttn_Start.onHover = onHover;
            bttn_Start.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Start.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 - bttn_Start.currentTexture.Height/1.2f);
            gameObjectsList.Add(bttn_Start);

        }
        private void LoadEditorButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button_editor"), mainGame.Content.Load<Texture2D>("button_editor_hover") };
            bttn_Create = new Button(myButtonTextureList);
            bttn_Create.onClick = StartEditor;
            bttn_Create.onHover = onHover;
            bttn_Create.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Create.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 + bttn_Create.currentTexture.Height/3f);
            gameObjectsList.Add(bttn_Create);

        }

        private void LoadLoadButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button_load"), mainGame.Content.Load<Texture2D>("button_load_hover") };
            bttn_Load = new Button(myButtonTextureList);
            bttn_Load.onClick = StartSavedLevel;
            bttn_Load.onHover = onHover;
            bttn_Load.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Load.currentTexture.Width / 2, mainGame.Window.ClientBounds.Height / 2 + bttn_Load.currentTexture.Height*1.5f);
            if (File.Exists(AssetsManager.savedLevelsPath))
            {
                gameObjectsList.Add(bttn_Load);
            }


        }

        public void onHover(Button p_Button)
        {
            sndButton.Play();
        }
        public void StartGame(Button p_Button)
        {
            sndButton.Play();
            ServiceLocator.GetService<GameState>().currentLevelsJSON = AssetsManager.levelsPath;
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
  
        }

        public void StartSavedLevel(Button p_Button)
        {
            sndButton.Play();
            ServiceLocator.GetService<GameState>().currentLevelsJSON = AssetsManager.savedLevelsPath;
            mainGame.gameState.ChangeScene(GameState.SceneType.Gameplay);
        }

        public void StartEditor(Button p_Button)
        {
            sndButton.Play();
            mainGame.gameState.ChangeScene(GameState.SceneType.Editor);
        }
    }
}
