using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace CasseBrique
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Brique> mesBriques;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
  
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D myBubble = Content.Load<Texture2D>("bubble");
            initBriques();
            tapeAllBriques();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void initBriques()
        {
            mesBriques = new List<Brique>();
            bBubbleBrique bubbleBrique = new bBubbleBrique();
            bExplosingBrique explosingBrique = new bExplosingBrique();
            bFireBrique fireBrique = new bFireBrique();
            mesBriques.Add(bubbleBrique);
            mesBriques.Add(explosingBrique);
            mesBriques.Add(fireBrique);
        }

        private void tapeAllBriques()
        {
            foreach (Brique brique in mesBriques) 
            {
                brique.Tape();
            }
        }
    }
}