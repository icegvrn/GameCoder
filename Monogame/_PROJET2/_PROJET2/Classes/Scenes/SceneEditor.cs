
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;


namespace BricksGame
{
    internal class SceneEditor : Scene
    {
        private GameManager gameManager;
        private Song myMusic;
        private Texture2D grid;
        public List<Dice> listOfDices;
        private Gamesystem.dice currentDice;
        private Button previousDice;
        private Button nextDice;
        private float buttonsMargin = 40f;

        private LevelList listOfLevels;
        private List<int> tempDicesGrid;
        private DicesFactory dicesFactory;

        private int gridCol = 9;
        private int gridLines = 12;

        private BaseGrid gameGrid;
        public BaseGrid GameGrid { get { return gameGrid; } }

        private int currentNb;

        private MouseState oldMouseState;

        private Button bttn_Save;

        public SceneEditor(MainGame p_mainGame) : base(p_mainGame) 
        {
           
        }

        public override void Load()
        {
            CreateSavedLevelFile();
            LoadBackgroundImage();
            CreateSwitchDicesButtons();
            CreateDices();
            CreatePlayingArea();
            CreateBaseGrid(gridCol, gridLines);
            LoadDices();
            LoadSaveButton();

        }

        private void LoadSaveButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { mainGame.Content.Load<Texture2D>("button_save"), mainGame.Content.Load<Texture2D>("button_save_hover") };
            bttn_Save = new Button(myButtonTextureList);
            bttn_Save.onClick = Save;
            bttn_Save.onHover = onHover;
            bttn_Save.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - bttn_Save.currentTexture.Width / 2, 50);
            gameObjectsList.Add(bttn_Save);

        }

        private void CreateSavedLevelFile()
        {
            if (!File.Exists(AssetsManager.savedLevelsPath))
            {
                LevelList levelList = new LevelList();
                levelList.Levels[0].Dices = new int[gridLines][];

                for (int i = 0; i < gridLines; i++)
                {
                    levelList.Levels[0].Dices[i] = new int[gridCol];
                }

                string json = JsonSerializer.Serialize(levelList);
                File.WriteAllText(AssetsManager.savedLevelsPath, json);      
            }
          
        }

        private void LoadDices()
        {
            ReadLevelsData();
            InitDicesFactory();
            CreateDicesFromData(1, GameGrid);
        }

        private void InitDicesFactory() 
        {
            dicesFactory = new DicesFactory();
        } 

        private void CreateBaseGrid(int lin, int col)
        {
            gameGrid = new BaseGrid(lin, col);
        }

        private void CreatePlayingArea()
        {
            PlayerArea playingArea = new PlayerArea(63, 122, 483, 560);
            ServiceLocator.RegisterService(playingArea);
        }


        public override void UnLoad()
        {
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {

            if (GameKeyboard.IsKeyReleased(Keys.Space))
            {
                Save(bttn_Save);
            }

            GetSlotRectFromMouse();
            base.Update(gameTime);
        }

    
        private void GetSlotRectFromMouse()
        {
            MouseState newMouseState = Mouse.GetState();
            MouseState mouse = ServiceLocator.GetService<MouseState>();
            foreach (Rectangle rectangle in GameGrid.SlotRectangles)
            {
                if (rectangle.Contains(mouse.X, mouse.Y) && rectangle.Y <= gameGrid.maxDestination)
                {
                    int slotIndex = GameGrid.GetSlotIndexFromPosition(new Vector2(rectangle.X, rectangle.Y));

                    if (newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {
                        UpdateDiceNumber(slotIndex, currentNb);
                    }
                    else if (newMouseState != oldMouseState && newMouseState.RightButton == ButtonState.Pressed)
                    {
                        UpdateDiceNumber(slotIndex, 0);
                    }
                }
            } 
            oldMouseState = newMouseState;
        }

    

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.LightGray);   
            mainGame._spriteBatch.DrawString(AssetsManager.MainFont, "E-D-I-T-O-R M-O-D-E", new Vector2(mainGame.Window.ClientBounds.Width / 2 - 110, 1), Color.White);
            GameGrid.DrawSlots(mainGame._spriteBatch, false);  
            base.Draw(gameTime);
        }

        public void CreateDicesList()
        {
            listOfDices = new List<Dice>();
            Dice d3 = new Dice(Gamesystem.dice.d3, false);
            Dice d4 = new Dice(Gamesystem.dice.d4, false);
            Dice d6 = new Dice(Gamesystem.dice.d6, false);
            Dice d8 = new Dice(Gamesystem.dice.d8, false);
            Dice d10 = new Dice(Gamesystem.dice.d10, false);
            Dice d12 = new Dice(Gamesystem.dice.d12, false);
            Dice d20 = new Dice(Gamesystem.dice.d20, false);
            listOfDices.Add(d3);
            listOfDices.Add(d4);
            listOfDices.Add(d6);
            listOfDices.Add(d8);
            listOfDices.Add(d10);
            listOfDices.Add(d12);
            listOfDices.Add(d20);
        }


        public void AddDices()
        {
            Rectangle Screen = mainGame.Window.ClientBounds;
            for (int i = 0; i < listOfDices.Count; i++)
            {
                listOfDices[i].Position = new Vector2(Screen.Width/2, Screen.Height - listOfDices[i].BoundingBox.Height);
                AddToGameObjectsList(listOfDices[i]);
            }
        }

        public void SetToDice(Gamesystem.dice d)
        {
            currentDice = d;

            foreach (Dice dice in listOfDices)
            {

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
                    currentNb = 4;
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d6);
                    currentNb = 6;
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d8);
                    currentNb = 8;
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d10);
                    currentNb = 10;
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d12);
                    currentNb = 12;
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d20);
                    currentNb = 20;
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d3);
                    currentNb = 3;
                    break;
                 default:
                    SetToDice(Gamesystem.dice.d3);
                    currentNb = 3;
                    break;
            }
        }

        public void PreviousDice(Button p_Button)
        {
     
            switch (currentDice)
            {
                case Gamesystem.dice.d3:
                    SetToDice(Gamesystem.dice.d20);
                    currentNb = 20;
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d3);
                    currentNb = 3;
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d4);
                    currentNb = 4;
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d6);
                    currentNb = 6;
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d8);
                    currentNb = 8;
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d10);
                    currentNb = 10;
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d12);
                    currentNb = 12;
                    break;
                default:
                    SetToDice(Gamesystem.dice.d3);
                    currentNb = 3;
                    break;
            }

        }

        private void CreateDices()
        {
            CreateDicesList();
            AddDices();
            SetToDice(Gamesystem.dice.d3);
            currentNb = 3;
        }

        private void CreateSwitchDicesButtons()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(mainGame.Content.Load<Texture2D>("arrow"));
            LoadPreviousDiceButton(myButtonTextureList);
            LoadNextDiceButton(myButtonTextureList);
           
        }

        private void LoadPreviousDiceButton(List<Texture2D> myButtonTextureList)
        {
            previousDice = new Button(myButtonTextureList);
            previousDice.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 - previousDice.currentTexture.Width - buttonsMargin,  mainGame.Window.ClientBounds.Height - previousDice.currentTexture.Height*2);
            previousDice.onClick = PreviousDice;
            previousDice.onHover = OnHover;
            gameObjectsList.Add(previousDice);
      
        }

        private void LoadNextDiceButton(List<Texture2D> myButtonTextureList)
        {
           
            nextDice = new Button(myButtonTextureList);
            nextDice.Position = new Vector2(mainGame.Window.ClientBounds.Width / 2 + buttonsMargin, mainGame.Window.ClientBounds.Height - previousDice.currentTexture.Height*2);
            nextDice.onClick = NextDice;
            nextDice.onHover = OnHover;
            nextDice.ChangeSpriteEffects(SpriteEffects.FlipHorizontally);
            gameObjectsList.Add(nextDice);
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

        public void OnHover(Button p_Button)
        {

        }

        private void LoadBackgroundImage()
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            background = content.Load<Texture2D>("images/map1");
        }


        private void ReadLevelsData()
        {
            string json = File.ReadAllText(AssetsManager.savedLevelsPath);
            listOfLevels = JsonSerializer.Deserialize<LevelList>(json);
        }

        private void CreateDicesFromData(int level, BaseGrid grid)
        {
            CreateDices(grid, ReadDicesData(level));
        }

        private List<int> ReadDicesData(int level)
        {
            Level levelData = listOfLevels.Levels[level - 1];
           tempDicesGrid = new List<int>();

            foreach (int[] lines in levelData.Dices)
            {
                foreach (int dice in lines)
                {
                    tempDicesGrid.Add(dice);
                }
            }
            return tempDicesGrid;
        }

        private void DrawDicesNumber()
        {
            Level levelData = listOfLevels.Levels[0];
            List<int> levelDices = new List<int>();

            int nb = 0;
            foreach (int[] lines in levelData.Dices)
            {
                foreach (int dice in lines)
                {
                    mainGame._spriteBatch.DrawString(AssetsManager.MainFont, dice.ToString(), GameGrid.GetPositionFromGrid(nb), Color.White);
                    nb++;
                }
            }
        }

        private void UpdateDiceNumber(int index, int nb)
        {
           tempDicesGrid[index] = nb;
           GameGrid.Clear();
           CreateDices(GameGrid, tempDicesGrid);
        }

        private void CreateDices(BaseGrid grid, List<int> dices)
        {
            List<Dice> dicesList = dicesFactory.Load(dices, false);
            for (int n = 0; n < dicesList.Count; n++)
            {
                grid.AddBrickable(dicesList[n], n);
                ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(dicesList[n]);
            }
        }

        private void onHover(Button p_Button)
        {

        }
        private void Save(Button p_Button)
        {
            List<int[]> array = new List<int[]>();
            int col = 0;
            bool empty = true;
            
            for (int i=0; i < tempDicesGrid.Count; i++)
            {
              
                if (i % GameGrid.WidthInColumns == 0)
                {  
                    int[] colArray = new int[GameGrid.WidthInColumns];
                    array.Add(colArray);
                    col = 0;
                }

                array[array.Count - 1][col] = tempDicesGrid[i];
                col++;

                if (tempDicesGrid[i] != 0) {
                    empty = false;
                }
            }

            if (empty)
            {
                int[] tempArray = new int[GameGrid.WidthInColumns];
                tempArray[0] = 3;
                array.RemoveAt(0);
                array.Insert(0, tempArray);
            }

            int[][] level = array.ToArray();
       
            listOfLevels.Levels[0].Dices = level;   
            string json = JsonSerializer.Serialize(listOfLevels);
            File.WriteAllText(AssetsManager.savedLevelsPath, json);
            mainGame.gameState.ChangeScene(GameState.SceneType.Menu);
            

        }

    }


}
