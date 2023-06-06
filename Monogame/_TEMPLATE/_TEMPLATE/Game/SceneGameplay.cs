using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE
{
    internal class SceneGameplay : Scene
    {
        private Hero MyShip;
        private Song myMusic;
        public SceneGameplay(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
           
            Rectangle Screen = mainGame.Window.ClientBounds;
            List<Texture2D> myShipTextureList = new List<Texture2D>();
            myShipTextureList.Add(mainGame.Content.Load<Texture2D>("ship"));
            MyShip = new Hero(myShipTextureList);
            listActors.Add(MyShip);
            myMusic = AssetsManager.gamePlayMusic;
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(myMusic);
            base.Load();
        }

        public override void UnLoad()
        {
            MediaPlayer.Stop();
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {
            MyShip.Move(1,0.2f);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the Gameplay !", new Vector2(1, 1), Color.White);
            base.Draw(gameTime);
        }
    }
}
