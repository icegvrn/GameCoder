
using BricksGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class Dice : Sprite, IBrickable, IDestroyable
    {
        public float GridSlotNb { get; set; }
        Texture2D diceSheet;
        List<Rectangle> tiles;
        int currentNb = 1;
        public int facesNb { get; private set; }
        int lastMathCeil;
        Random rand;
       public bool IsDestroy { get; set; }
        float diceAnimationTimer = 0f;
        float diceSpeed = 3.5f;
        bool diceAnimationStarted = false;
        public bool DiceRolled { get;  private set; }
        public int DiceResult { get { return currentNb; } }
        bool isHover = false;
        MouseState oldMouseState;
        Vector2 diceScale = new Vector2(0.8f, 0.8f);

        public Dice(Gamesystem.dice value) :  base()
        { 
            rand = new Random();
           ContentManager content = ServiceLocator.GetService<ContentManager>();
            List<Texture2D> spritesTexture = new List<Texture2D>();
            tiles = new List<Rectangle>();
           
            switch (value)
            {
                case Gamesystem.dice.d3:
                    facesNb = 3;
                    diceSheet = content.Load<Texture2D>("images/d3");
                    break;

                case Gamesystem.dice.d4 :
                    facesNb = 4;
                    diceSheet = content.Load<Texture2D>("images/d4");
                    break;

                case Gamesystem.dice.d6:
                    facesNb = 6;
                    diceSheet = content.Load<Texture2D>("images/d6");
                    break;

                case Gamesystem.dice.d8:
                    facesNb = 8;
                    diceSheet = content.Load<Texture2D>("images/d8");
                    break;

                case Gamesystem.dice.d10:
                    facesNb = 10;
                    diceSheet = content.Load<Texture2D>("images/d10");
                    break;

                case Gamesystem.dice.d12:
                    facesNb = 12;
                    diceSheet = content.Load<Texture2D>("images/d12");
                    break;

                case Gamesystem.dice.d20:
                    facesNb = 20;
                    diceSheet = content.Load<Texture2D>("images/d20");
                    break;

                default:
                    facesNb = 0;
                    diceSheet = null;
                    break;
            }

            if (diceSheet != null)
            {
                for (int i = 0; i < facesNb; i++)
                {
                    Rectangle newRect = new Rectangle(diceSheet.Width/facesNb * i, 0, diceSheet.Width / facesNb, diceSheet.Height);
                    tiles.Add(newRect);
                }
                BoundingBox = new Rectangle((int)Position.X - tiles[currentNb - 1].Width / 2, (int)Position.Y - tiles[currentNb - 1].Height / 2, tiles[currentNb - 1].Width, tiles[currentNb - 1].Height);
            }


        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (diceSheet != null)
            {
                p_SpriteBatch.Draw(diceSheet, Position, tiles[currentNb - 1], Color.White, 0, new Vector2(tiles[currentNb - 1].Width / 2, tiles[currentNb - 1].Height / 2), diceScale, SpriteEffects.None, 1f);
            }


        }

        public override void Update(GameTime p_GameTime)
        {
            if (diceSheet != null)
            {
                BoundingBox = new Rectangle((int)Position.X - tiles[currentNb - 1].Width / 2, (int)Position.Y - tiles[currentNb - 1].Height / 2, tiles[currentNb - 1].Width, tiles[currentNb - 1].Height);

                CheckIfClicked();

                if (diceAnimationStarted)
                {
                    AnimateTheDice(p_GameTime);

                }
            }
     
        }

        public void CheckIfClicked()
        {
            if (!DiceRolled)
            {
                MouseState newMouseState = Mouse.GetState();
                Point MousePos = newMouseState.Position;

                if (BoundingBox.Contains(MousePos))
                {

                    if (!isHover)
                    {
                        diceScale = Vector2.One;
                        isHover = true;

                    }
                    else
                    {
                        if (newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {
                            // Debug.WriteLine("Clic !");
                            RollDice();
                        }
                    }
                }
                else
                {
                    diceScale = new Vector2(0.8f, 0.8f);
                    isHover = false;
                }
                oldMouseState = newMouseState;
            }
          
        }

        public void AddToDice(int nb)
        {
            if (currentNb < tiles.Count())
            {
                currentNb += nb;
            }
        }

        public void SubsToDice(int nb)
        {
            if (currentNb > 1)
            {
                currentNb -= nb;
            }
        }

        //remplacer par une anim
        public void RollDice()
        {
            diceAnimationStarted = true;
        }

        private void AnimateTheDice(GameTime p_GameTime)
        {
            diceAnimationTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds * diceSpeed;

            if (diceAnimationTimer > 5f + facesNb/4)
            {
                currentNb = rand.Next(1, facesNb + 1);
                diceAnimationStarted = false;
                diceAnimationTimer = 0f;
                DiceRolled = true;
            }

            else if ((int)Math.Ceiling(diceAnimationTimer) != lastMathCeil)
            {
                currentNb = rand.Next(1, facesNb + 1);
            }

            lastMathCeil = (int)Math.Ceiling(diceAnimationTimer);
        }

        public void Destroy(IDestroyable dice)
        {
            IsDestroy = true;
            DiceRolled = false;
            dice = null;
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}
