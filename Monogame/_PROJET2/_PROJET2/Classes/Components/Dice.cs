
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BricksGame
{
    public class Dice : Sprite, IBrickable, IDestroyable
    {

        // Apparence du dé
        protected Texture2D diceSheet;
        private Vector2 diceScale = new Vector2(0.8f, 0.8f);
        private bool isVisible = true;
        private List<Rectangle> tiles;

        // Animation du dé
        private float diceAnimationTimer = 0f;
        private float diceSpeed = 3.5f;
        public bool DiceAnimationStarted { get; set; }

        // Etats du dés 
        private bool isHover = false;
        public bool IsRollable { get; private set; }
        public bool DiceRolled { get; protected set; }
        public bool IsDestroy { get; set; }
        
        // Valeurs
        private Random rand;
        private int currentNb = 1;
        private int lastMathCeil;
        public Gamesystem.dice value { get; private set; }
        public int DiceResult { get { return currentNb; } }
        public int facesNb { get; protected set; }

        // Emplacement sur grille
        public float GridSlotNb { get; set; }

        // Son
        private SoundEffect sndRoll;
        private SoundEffect sndHover;


        public Dice(Gamesystem.dice p_value, bool rollable) : base()
        {
            IsRollable = rollable;
            InitComponents();
            InitSounds();
            InitDice(p_value);
            InitTile();
        }

        // A l'update, animation du dés si l'animation est lancée. Update de la boundingBox.
        public override void Update(GameTime p_GameTime)
        {
            if (diceSheet != null)
            {
                BoundingBox = new Rectangle((int)Position.X - tiles[currentNb - 1].Width / 2, (int)Position.Y - tiles[currentNb - 1].Height / 2, tiles[currentNb - 1].Width, tiles[currentNb - 1].Height);

                CheckIfClicked();

                if (DiceAnimationStarted)
                {
                    AnimeAndGetResult(p_GameTime);
                }
            }
        }

        // Si le dés est visible on le dessine.
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (diceSheet != null && isVisible)
            {
                p_SpriteBatch.Draw(diceSheet, Position, tiles[currentNb - 1], Color.White, 0, new Vector2(tiles[currentNb - 1].Width / 2, tiles[currentNb - 1].Height / 2), diceScale, SpriteEffects.None, 1f);
            }
        }

        public void InitComponents()
        {
            rand = new Random();
            tiles = new List<Rectangle>();
        }
        public void InitSounds()
        {
            sndRoll = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(ServiceLocator.GetService<IPathsService>().GetSoundsRoot() + "diceRoll");
            sndHover = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(ServiceLocator.GetService<IPathsService>().GetSoundsRoot() + "diceHover");
        }

        // Initialisation du dés en lui attribuant un nombre de face et en allant chercher la bonne image qui correspond.
        public void InitDice(Gamesystem.dice p_value)
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            string imgPath = ServiceLocator.GetService<IPathsService>().GetDicesImagesPathRoot();
            switch (p_value)
            {
                case Gamesystem.dice.d3:
                    value = Gamesystem.dice.d3;
                    facesNb = 3;
                    diceSheet = content.Load<Texture2D>(imgPath + "d3");
                    break;

                case Gamesystem.dice.d4:
                    value = Gamesystem.dice.d4;
                    facesNb = 4;
                    diceSheet = content.Load<Texture2D>(imgPath + "d4");
                    break;

                case Gamesystem.dice.d6:
                    value = Gamesystem.dice.d6;
                    facesNb = 6;
                    diceSheet = content.Load<Texture2D>(imgPath + "d6");
                    break;

                case Gamesystem.dice.d8:
                    value = Gamesystem.dice.d8;
                    facesNb = 8;
                    diceSheet = content.Load<Texture2D>(imgPath + "d8");
                    break;

                case Gamesystem.dice.d10:
                    value = Gamesystem.dice.d10;
                    facesNb = 10;
                    diceSheet = content.Load<Texture2D>(imgPath + "d10");
                    break;

                case Gamesystem.dice.d12:
                    value = Gamesystem.dice.d12;
                    facesNb = 12;
                    diceSheet = content.Load<Texture2D>(imgPath + "d12");
                    break;

                case Gamesystem.dice.d20:
                    value = Gamesystem.dice.d20;
                    facesNb = 20;
                    diceSheet = content.Load<Texture2D>(imgPath + "d20");
                    break;

                case Gamesystem.dice.dMagic:
                    value = Gamesystem.dice.dMagic;
                    facesNb = 6;
                    diceSheet = content.Load<Texture2D>(imgPath + "dMagic");
                    break;

                default:
                    facesNb = 0;
                    diceSheet = null;
                    break;
            }
        }

        // Permet de croper correctement la tilesheet des dés. 
        public void InitTile()
        {
            if (diceSheet != null)
            {
                for (int i = 0; i < facesNb; i++)
                {
                    Rectangle newRect = new Rectangle(diceSheet.Width / facesNb * i, 0, diceSheet.Width / facesNb, diceSheet.Height);
                    tiles.Add(newRect);
                }
                BoundingBox = new Rectangle((int)Position.X - tiles[currentNb - 1].Width / 2, (int)Position.Y - tiles[currentNb - 1].Height / 2, tiles[currentNb - 1].Width, tiles[currentNb - 1].Height);
            }
        }

        // Si le dés est survolé, on change son échelle, s'il est cliqué, on lance le Roll du dé si ce n'est pas déjà fait.
        public void CheckIfClicked()
        {
            if (!DiceRolled)
            {
                if (BoundingBox.Contains(ServiceLocator.GetService<IInputService>().GetMousePosition()))
                {

                    if (!isHover && !ServiceLocator.GetService<IInputService>().OnActionDown())
                    {
                        diceScale = Vector2.One;
                        isHover = true;
                        sndHover.Play();
                    }
                    else
                    {
                        if (ServiceLocator.GetService<IInputService>().OnActionDown() && IsRollable)
                        {
                            RollDice();
                        }
                    }
                }
                else
                {
                    diceScale = new Vector2(0.8f, 0.8f);
                    isHover = false;
                }

            }
        }

        // Lance l'animation du dé
        public void RollDice()
        {
            DiceAnimationStarted = true;
        }

        // Anmation du dés avec lecture du son, à la fin choix d'un chiffre aléatoire.
        private void AnimeAndGetResult(GameTime p_GameTime)
        {
            diceAnimationTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds * diceSpeed;

            if (diceAnimationTimer > 5f + facesNb / 4)
            {
                sndRoll.Play();
                currentNb = rand.Next(1, facesNb + 1);
                DiceAnimationStarted = false;
                diceAnimationTimer = 0f;
                DiceRolled = true;
            }

            else if ((int)Math.Ceiling(diceAnimationTimer) != lastMathCeil)
            {
                currentNb = rand.Next(1, facesNb + 1);
                sndRoll.Play();
            }

            lastMathCeil = (int)Math.Ceiling(diceAnimationTimer);
        }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        // Non utilisée : permet d'ajouter un chiffre au dé
        public void AddToDice(int nb)
        {
            if (currentNb < tiles.Count())
            {
                currentNb += nb;
            }
        }

        // Non utilisée : permet d'enlever un chiffre au dé
        public void SubsToDice(int nb)
        {
            if (currentNb > 1)
            {
                currentNb -= nb;
            }
        }

        // Méthode de destruction du dé, appelle la méthode Destroy sur IDestroyable
        public void Destroy()
        {
            Destroy(this);
        }

        public void Destroy(IDestroyable dice)
        {
            IsDestroy = true;
            DiceRolled = false;
            dice = null;
        }
    }
}
