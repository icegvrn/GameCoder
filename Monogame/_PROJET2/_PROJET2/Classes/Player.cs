using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;


namespace BricksGame
{
    public class Player : Sprite, ICollider
    {
        // Gestion des balls
        private List<Ball> balls;
        public List<Ball> BallsList { get { return balls; } private set { balls = value; } }

        public bool isCollide { private set; get; }

        private bool isHit = false;
        private float _speed = 15;
        public override float Speed { get { return _speed; } set { _speed = value; } }
        private Animator animator;
        private List<Texture2D> textures;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterDirection currentDirection;
        
        public Vector2 Size;
        private Color playerColor = Color.White;

        //Vie du joueur
        public bool IsDead = false;
        public bool IsReady = false;

        //Points du joueur
        private float maxPoints; 
        private float initialLife;

        // Barres du joueur
        private int barsLenght = 100;
        private int barsHeight = 12;
        // Barre de vie
        private EvolutiveColoredGauge barLife; 
        private Texture2D lifeIcon;
        private Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
        float[] threshold = { 0.65f, 0.55f, 0.35f };
       // Barre de points
        private Texture2D pointsIcon;
        private ColoredGauge pointsBar;

        private float provisoryLife;

        private float timer = 0.2f;

        private bool isBlinking = false;
        private float blinkTimer = 0f;
        //Portrait
        private Texture2D portrait;

        //Ball count
        private int munitionNb = 10;
        private int baseMunition = 10;
        public bool HasMunition { get { if (munitionNb <= 0) { return false; } else { return true; } } }

        //Etat du joueur
        private float destroyDelay = 2f;
        private float destroyTimer = 0f;
        public bool startDying;

        private SoundManager soundContainer;
        private SoundEffect sndCriticalLife;
        private bool criticalLifeAnnounced;

        private MouseState oldMouseState;

        private bool hasFired;



        private Texture2D munitionCounter;
        public Player(Texture2D p_texture) : base(p_texture)
        {

            soundContainer = new SoundManager(this);
            sndCriticalLife = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>("Sounds/critical_life");
            Speed = 15;
            currentState = Gamesystem.CharacterState.idle;
            currentDirection = Gamesystem.CharacterDirection.right;
            textures = new List<Texture2D>();
            animator = new Animator(this, 0.15f);
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
            Reset();

            // Vie
            lifeIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_heart");
            initialLife = 3000f;
            provisoryLife = initialLife;
            PlayerState.SetLife((int)initialLife);

            Rectangle lifeBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            barLife = new EvolutiveColoredGauge(initialLife, lifeBar, Color.White, threshold, lifeColors);

            //Points
            pointsIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_power");
            PlayerState.SetPoints(0);
            maxPoints = 3000f;

           
            Rectangle rectPointsBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width/2 - 50 - barsLenght - pointsIcon.Width/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            pointsBar = new ColoredGauge(maxPoints, rectPointsBar, Color.CornflowerBlue);
            
          
            // Portrait
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/character_portrait");

            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_counter_true");
            BallsList = new List<Ball>();

        }

        public void ChangeState(Gamesystem.CharacterState state)
        {
         currentState = state;
         animator.ChangeState(state);
        }

        public override void Update(GameTime p_GameTime)
        {

            if (PlayerState.Life != provisoryLife)
            {
                if (PlayerState.Life > 0)
                {
                    PlayerState.SubsLife(1); 
                        
                }
            }
            if (IsReady)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Move(-1, 0);
                    currentDirection = Gamesystem.CharacterDirection.left;
                    ChangeState(Gamesystem.CharacterState.l_walk);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Move(1, 0);
                    currentDirection = Gamesystem.CharacterDirection.right;
                    ChangeState(Gamesystem.CharacterState.walk);
                }
           

                MouseState newMouseState = Mouse.GetState();
                Point MousePos = newMouseState.Position;

                if ((newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed) || GameKeyboard.IsKeyReleased(Keys.Space))
                {   
                    if (!hasFired)
                    {
                        if (currentDirection == Gamesystem.CharacterDirection.left)
                        {
                            ChangeState(Gamesystem.CharacterState.l_fire);
                        }
                        else
                        {
                            ChangeState(Gamesystem.CharacterState.fire);
                        }
                        Fire(BallsList[0]);
                        RemoveMunition(1);
                        hasFired = true;
                        if (currentDirection == Gamesystem.CharacterDirection.left)
                        {
                            ChangeState(Gamesystem.CharacterState.l_idle);
                        }
                        else
                        {
                            ChangeState(Gamesystem.CharacterState.idle);
                        }
                    }
                    

                  
                }
                oldMouseState = newMouseState;


              
                if (GameKeyboard.IsKeyReleased(Keys.Q) || GameKeyboard.IsKeyReleased(Keys.Left))
                {
                   ChangeState(Gamesystem.CharacterState.l_idle);
                }
                else if (GameKeyboard.IsKeyReleased(Keys.D)  ||GameKeyboard.IsKeyReleased(Keys.Left))
                {
                    ChangeState(Gamesystem.CharacterState.idle);
                }

                if (PlayerState.Life <= 0)
                {
                    if ((currentState != Gamesystem.CharacterState.die || currentState != Gamesystem.CharacterState.l_die) && !startDying)
                    {
                        animator.SetLoop(false);
                        destroyTimer = 0f;
                        startDying = true;
                       
                        if (currentDirection == Gamesystem.CharacterDirection.left)
                        {
                            ChangeState(Gamesystem.CharacterState.l_die);
                        }
                        else
                        {
                            ChangeState(Gamesystem.CharacterState.die);
                        }
                        soundContainer.Play(Gamesystem.CharacterState.die);
                    }
                    
                if (startDying)
                    {
                        if (destroyTimer >= destroyDelay)
                        {
                            IsDead = true;
                        }
                        else
                        {
                            destroyTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
                        }
                    }
                   
                
                  
      
                 
                }

                else if (PlayerState.Life <= initialLife/4)
                {
                    if (!criticalLifeAnnounced)
                    {
                        sndCriticalLife.Play();
                        criticalLifeAnnounced = true;
                    }
                    
                }

               

                if (isHit)
                {
                    BlinkOnHit(p_GameTime, true);
                }

                else
                {
                    BlinkOnHit(p_GameTime, false);
                }

              
            }

            barLife.Update(p_GameTime, PlayerState.Life, barLife.Position);
            pointsBar.CurrentValue = PlayerState.Points;
            pointsBar.Update(p_GameTime);

            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y/3); 
            animator.Update(p_GameTime);

        }



        public override void Move(float p_x, float p_y)
        {
            if (Position.X + p_x * Speed < ServiceLocator.GetService<PlayerArea>().area.X) 
            {
                Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.X, Position.Y);
            }
                
            else if ((Position.X + p_x * Speed) > ServiceLocator.GetService<PlayerArea>().area.Right - Size.X)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayerArea>().area.Right - Size.X, Position.Y);
            }

            else
            {
             base.Move(p_x, p_y);
            }
            
        }

        public void EndOfTouch()
        {
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {

            animator.Draw(p_SpriteBatch, Position, playerColor);
           // DrawBoundingBox(p_SpriteBatch);

            barLife.Draw(p_SpriteBatch);
            p_SpriteBatch.Draw(lifeIcon, new Vector2((barLife.Position.X + barsLenght) - lifeIcon.Width*0.5f, barLife.Position.Y - lifeIcon.Height/3), Color.White);

            pointsBar.Draw(p_SpriteBatch);
            p_SpriteBatch.Draw(pointsIcon, new Vector2((pointsBar.Position.X + barsLenght) - pointsIcon.Width * 0.5f, pointsBar.Position.Y - pointsIcon.Height/2), Color.White);

            //Draw portrait
            Vector2 portraitPosition = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - portrait.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - portrait.Height * 1.25f);
            p_SpriteBatch.Draw(portrait, portraitPosition, Color.White);

            //Draw Counter 
            for (int n = 0; n < munitionNb; n++)
            {
                p_SpriteBatch.Draw(munitionCounter, new Vector2(portraitPosition.X + 6 *(n), portraitPosition.Y + portrait.Height + 2) , Color.White);
            }
            
        }

        public void TouchedBy(GameObject p_By)
        {
            if (p_By is Ball)
            {
              //  Debug.WriteLine("PLAYER : J'ai touché la balle !");
            }

      
        }
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D boxTexture = AssetsManager.blankTexture;
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);


            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

        public void Reset()
        {
            Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - Size.X/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - Size.Y*1.8f);
            playerColor = Color.White;
            
            if (BallsList is not null)
            {
                foreach (Ball ball in BallsList)
                {
                    ball.Destroy();
                    ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(ball);
                }

            }
            BallsList = new List<Ball>();
        }

        public void ResetMunition()
        {
            munitionNb = baseMunition;
        }

  

        public void RemoveMunition(int nb)
        {
            munitionNb -= nb;
        }

        public void Fire(Ball ball)
        {
            ball.Fire();
        }

        public void Prepare()
        {
            CreateNewBall();
            hasFired = false;
        }

        public void CreateNewBall()
        {
            List<Texture2D> myBallTextureList = new List<Texture2D>();
            myBallTextureList.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/ball"));
            Ball ball = new Ball(myBallTextureList);
            ball.Position = new Vector2(Position.X + Size.X / 2, Position.Y - 20f);
            ball.Following(this);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(ball);
            BallsList.Add(ball);
    
            
        }

        public void IsHit(ICollider h_By, int hitForce)
        {
            if (h_By is Monster)
            {
                isHit = true;
                provisoryLife = PlayerState.Life - hitForce;
             
            }
        }
        public void BlinkOnHit(GameTime p_GameTime, bool b)
        {
            if (b)
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    blinkTimer = 0f;
                }

                if (isBlinking)
                {
                    blinkTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;

                    if (blinkTimer >= 2f)
                    {
                        isBlinking = false;
                        isHit = false;
                        playerColor = Color.White;
                    }

                    else
                    {
                        if (blinkTimer % 1f <= 0.5f)
                        {
                            playerColor = Color.White;
                        }

                        else
                        {
                            playerColor = Color.Red;
                        }

                    }
                }
            }

            else
            {
                playerColor = Color.White;
            }
        }

        public Rectangle NextPositionX()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point((int)(Speed), 0));
            return nextPosition;
        }

        public Rectangle NextPositionY()
        {
            Rectangle nextPosition = BoundingBox;
            nextPosition.Offset(new Point(0, (int)(Speed)));
            return nextPosition;
        }

    }
}
