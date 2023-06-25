using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    internal class SceneGameOver : Scene
    {

        public SceneGameOver(MainGame p_mainGame) : base(p_mainGame) 
        { 
        }

        public override void Load()
        {
            base.Load();
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
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the Gameover !", new Vector2(1, 1), Color.White);
            base.Draw(gameTime);
        }
    }
}
