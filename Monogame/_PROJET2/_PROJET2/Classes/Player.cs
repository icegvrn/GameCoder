using BricksGame.Classes;
using Microsoft.Xna.Framework;
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

    
        private bool isHit = false;
        private float _speed = 15;
        public override float Speed { get { return _speed; } set { _speed = value; } }
        private Animator animator;
        private List<Texture2D> textures;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterState lastState;
        
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



        private float timer = 0.2f;

        private bool isBlinking = false;
        private float blinkTimer = 0f;
        //Portrait
        private Texture2D portrait;

        //Ball count
        private int munitionNb = 10;
        public bool HasMunition { get { if (munitionNb <= 0) { return false; } else { return true; } } }


        private Texture2D munitionCounter;
        public Player(List<Texture2D> p_texture) : base(p_texture)
        {
     
            Speed = 15;
            currentState = Gamesystem.CharacterState.idle;
            lastState = currentState;
            textures = new List<Texture2D>();
            textures = p_texture;
            animator = new Animator(p_texture[(int)Gamesystem.CharacterState.idle], 0.15f);
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
            Reset();

            // Vie
            lifeIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_heart");
            initialLife = 3000f;
        
            PlayerState.SetLife((int)initialLife);
            Rectangle lifeBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            barLife = new EvolutiveColoredGauge(initialLife, lifeBar, Color.White, threshold, lifeColors);

            //Points
            pointsIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_power");
            maxPoints = 3000f;

           
            Rectangle rectPointsBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width/2 - 50 - barsLenght - pointsIcon.Width/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, barsLenght, barsHeight);
            pointsBar = new ColoredGauge(maxPoints, rectPointsBar, Color.Purple);
            
          
            // Portrait
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/character_portrait");

            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_counter_true");
            BallsList = new List<Ball>();

        }

        public override void Update(GameTime p_GameTime)
        {
            if (IsReady)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    Move(-1, 0);
                    ChangeState(Gamesystem.CharacterState.l_walk);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    Move(1, 0);
                    ChangeState(Gamesystem.CharacterState.walk);
                }
                else if (GameKeyboard.IsKeyReleased(Keys.Space))
                {
                    Fire(BallsList[0]);
                    RemoveMunition(1);
                    ChangeState(Gamesystem.CharacterState.idle);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    ChangeState(Gamesystem.CharacterState.fire);
                }
                if (GameKeyboard.IsKeyReleased(Keys.Q))
                {
                    ChangeState(Gamesystem.CharacterState.l_idle);
                }
                else if (GameKeyboard.IsKeyReleased(Keys.D))
                {
                    ChangeState(Gamesystem.CharacterState.idle);
                }




                if (PlayerState.Life <= 0)
                {
                    IsDead = true;
                }

                barLife.CurrentValue = PlayerState.Life;
                barLife.Update(p_GameTime);
                pointsBar.CurrentValue = PlayerState.Points;
                pointsBar.Update(p_GameTime);


                if (lastState != currentState)
                {
                    animator.ChangeSpriteSheet(textures[(int)currentState]);
                    lastState = currentState;
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
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y/3);
            animator.Update(p_GameTime);





        }



        public override void Move(float p_x, float p_y)
        {
            if (Position.X + p_x * Speed < ServiceLocator.GetService<PlayingAera>().aera.X) 
            {
                Position = new Vector2(ServiceLocator.GetService<PlayingAera>().aera.X, Position.Y);
            }
                
            else if ((Position.X + p_x * Speed) > ServiceLocator.GetService<PlayingAera>().aera.Right - Size.X)
            {
                Position = new Vector2(ServiceLocator.GetService<PlayingAera>().aera.Right - Size.X, Position.Y);
            }

            else
            {
             base.Move(p_x, p_y);
            }
            
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
                Debug.WriteLine("PLAYER : J'ai touché la balle !");
            }

      
        }
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });

            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
          
        }

        public void Reset()
        {
            Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - Size.X/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - Size.Y*2f);
            playerColor = Color.White;
            

            if (BallsList is not null)
            {
                foreach (Ball ball in BallsList)
                {
                    ball.Destroy();
                }

            }

            BallsList = new List<Ball>();
        }

        public void ChangeState(Gamesystem.CharacterState state )
        {
            currentState = state;
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
        }

        public void CreateNewBall()
        {
            if (!HasMunition)
            {
                ServiceLocator.GetService<Scene>().End();
            }

            List<Texture2D> myBallTextureList = new List<Texture2D>();
            myBallTextureList.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/ball"));
            Ball ball = new Ball(myBallTextureList);
            ball.Position = new Vector2(Position.X + Size.X / 2, Position.Y - 20f);
            ball.Following(this);
            ServiceLocator.GetService<Scene>().AddToGameObjectsList(ball);
            BallsList.Add(ball);
    
            
        }

        public void IsHit(ICollider h_By, int hitForce)
        {
            if (h_By is Monster)
            {
                isHit = true;
                PlayerState.SubsLife(hitForce);
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


    }
}
