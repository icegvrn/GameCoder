using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace BricksGame
{
    internal class SceneEditor : Scene
    {
        private GameManager gameManager;
        private Texture2D background;
        private Texture2D grid;
        public List<DicePlaceholder> listOfDices;
        private Gamesystem.dice currentDice;
        private Button previousDice;
        private Button nextDice;
        private float buttonsMargin = 40f;
        public SceneEditor(MainGame p_mainGame) : base(p_mainGame) 
        {
           
        }

        public override void Load()
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            background = content.Load<Texture2D>("images/map1");
            grid = content.Load<Texture2D>("images/grid");
            CreateDicesList();
            AddDices();
            SetToDice(Gamesystem.dice.d3);
            Rectangle screen = mainGame.Window.ClientBounds;
            List<Texture2D> myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("arrow"));
            previousDice = new Button(myButtonTextureList);
            previousDice.Position = new Vector2(screen.Width / 2 - previousDice.currentTexture.Width - buttonsMargin, screen.Height - previousDice.currentTexture.Height * 1.5f);
            previousDice.onClick = PreviousDice;
            previousDice.onHover = null;
            nextDice = new Button(myButtonTextureList);
            nextDice.Position = new Vector2(screen.Width / 2 + buttonsMargin, screen.Height - previousDice.currentTexture.Height * 1.5f );
            nextDice.onClick = NextDice;
            nextDice.onHover = null;
            nextDice.ChangeSpriteEffects(SpriteEffects.FlipHorizontally);
            gameObjects.Add(nextDice);
            gameObjects.Add(previousDice);

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
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.LightGray);
            mainGame._spriteBatch.Draw(grid, Vector2.Zero, Color.White);
            DrawDices(mainGame._spriteBatch);
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "This is the Editor !", new Vector2(1, 1), Color.White);
            base.Draw(gameTime);
        }

        public void CreateDicesList()
        {
            listOfDices = new List<DicePlaceholder>();
            DicePlaceholder d3 = new DicePlaceholder(Gamesystem.dice.d3);
            DicePlaceholder d4 = new DicePlaceholder(Gamesystem.dice.d4);
            DicePlaceholder d6 = new DicePlaceholder(Gamesystem.dice.d6);
            DicePlaceholder d8 = new DicePlaceholder(Gamesystem.dice.d8);
            DicePlaceholder d10 = new DicePlaceholder(Gamesystem.dice.d10);
            DicePlaceholder d12 = new DicePlaceholder(Gamesystem.dice.d12);
            DicePlaceholder d20 = new DicePlaceholder(Gamesystem.dice.d20);
            listOfDices.Add(d3);
            listOfDices.Add(d4);
            listOfDices.Add(d6);
            listOfDices.Add(d8);
            listOfDices.Add(d10);
            listOfDices.Add(d12);
            listOfDices.Add(d20);
          
        }

        public void DrawDices(SpriteBatch p_spriteBatch)
        {
            Rectangle Screen = mainGame.Window.ClientBounds;
            for (int i=0; i < listOfDices.Count; i++)
            {
              //  p_spriteBatch.Draw(listOfDices[i], new Vector2(listOfDices[i].BoundingBox.Width*i, Screen.Height - listOfDices[i].BoundingBox.Height),  Color.White);
            }
        }

        public void AddDices()
        {
            Rectangle Screen = mainGame.Window.ClientBounds;
            for (int i = 0; i < listOfDices.Count; i++)
            {
                listOfDices[i].Position = new Vector2(Screen.Width/2, Screen.Height - listOfDices[i].BoundingBox.Height/2);
                AddToGameObjectsList(listOfDices[i]);
            }
        }

        public void SetToDice(Gamesystem.dice d)
        {
            currentDice = d;

            foreach (DicePlaceholder dice in  listOfDices)
            {
                Debug.WriteLine(dice.value);
                if (dice.value == d)
                {
                    dice.SetVisible(true);
                }
                else
                {
                    dice.SetVisible(false);
                }
            }  
        }

        public void NextDice(Button p_Button)
        {
         
            switch (currentDice)
            {
                case Gamesystem.dice.d3:
                   SetToDice(Gamesystem.dice.d4);
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d6);
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d8);
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d10);
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d12);
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d20);
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d3);
                    break;
                 default:
                    SetToDice(Gamesystem.dice.d3);
                    break;
            }
        }

        public void PreviousDice(Button p_Button)
        {
     
            switch (currentDice)
            {
                case Gamesystem.dice.d3:
                    SetToDice(Gamesystem.dice.d20);
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d3);
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d4);
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d6);
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d8);
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d10);
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d12);
                    break;
                default:
                    SetToDice(Gamesystem.dice.d3);
                    break;
            }

            }
        }
}
