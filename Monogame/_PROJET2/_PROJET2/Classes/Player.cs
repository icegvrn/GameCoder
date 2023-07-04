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

        public bool isHit = false;
        private float _speed = 15;
        public override float Speed { get { return _speed; } set { _speed = value; } }
        private List<Texture2D> textures;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterDirection currentDirection;

        public Vector2 Size;
 
        //Vie du joueur
        public bool IsDead = false;
        public bool IsReady = false;

        private float timer = 0.2f;

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

        private MouseState oldMouseState;

        private bool hasFired;

        private MagicalDice magicDice;

        private PlayerPowerManager playerPowerManager;
        private PlayerMouvement playerMouvement;
        private PlayerAnimator playerAnimator;
        private PlayerInput playerInput;
        private PlayerHealth playerHealth;
        

        private Texture2D munitionCounter;
        public Player(Texture2D p_texture) : base(p_texture)
        {
            playerPowerManager = new PlayerPowerManager(this);
            playerMouvement = new PlayerMouvement(this);
            playerAnimator = new PlayerAnimator(this, 0.15f);
            playerInput = new PlayerInput(this);
            playerHealth = new PlayerHealth(this, 500f);
          
            Speed = 15;
            currentState = Gamesystem.CharacterState.idle;
            currentDirection = Gamesystem.CharacterDirection.right;
            textures = new List<Texture2D>();
        
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
            Reset();

   
            // Portrait
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/character_portrait");

            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_counter_true");
            BallsList = new List<Ball>();

            magicDice = new MagicalDice();
            magicDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 125, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 80);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(magicDice);
            ChangeState(Gamesystem.CharacterState.idle);
        }

        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
        
        }

        public override void Update(GameTime p_GameTime)
        {
            playerAnimator.Update(p_GameTime);
            playerInput.Update(p_GameTime);
            playerHealth.Update(p_GameTime);
            playerPowerManager.Update(p_GameTime);

  
            if (IsReady)
            {
                if (playerHealth.IsDead)
                {
                    playerAnimator.Die();
                    Destroy(p_GameTime);      
                }
                else
                {
                    MouseState newMouseState = Mouse.GetState();
                    Point MousePos = newMouseState.Position;

                    playerPowerManager.Update(p_GameTime);

                    if ((newMouseState != oldMouseState && newMouseState.LeftButton == ButtonState.Pressed) || GameKeyboard.IsKeyReleased(Keys.Space))
                    {
                        if (!hasFired)
                        {
                            Action();

                            Fire(BallsList[0]);
                            RemoveMunition(1);
                            hasFired = true;

                            Stay();
 
                        }
                        else
                        {
                            playerPowerManager.ActivatePower();
                        }
                    }

                    oldMouseState = newMouseState;



                    if (isHit)
                    {
                       playerAnimator.Blink(p_GameTime, true);
                    }

                    else
                    {
                        playerAnimator.Blink(p_GameTime, false);
                    }

                }

                BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y / 3);
            }

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



        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            playerAnimator.Draw(p_SpriteBatch, Position);
            playerHealth.Draw(p_SpriteBatch);
            playerPowerManager.Draw(p_SpriteBatch);
          
            //Draw portrait
            Vector2 portraitPosition = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - portrait.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - portrait.Height * 1.25f);
            p_SpriteBatch.Draw(portrait, portraitPosition, Color.White);

            //Draw Counter 
            for (int n = 0; n < munitionNb; n++)
            {
                p_SpriteBatch.Draw(munitionCounter, new Vector2(portraitPosition.X + 6 * (n), portraitPosition.Y + portrait.Height + 2), Color.White);
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
            playerAnimator.Reset();
            Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - Size.X / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - Size.Y * 1.8f);
           

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
                playerHealth.Damage(hitForce);
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

        public void Left()
        {
            playerMouvement.Move(-1, 0);
            playerAnimator.ChangeDirection(Gamesystem.CharacterDirection.left);
            
        }

        public void Right()
        {
            playerMouvement.Move(1, 0);
            playerAnimator.ChangeDirection(Gamesystem.CharacterDirection.right);
        }

        public void Stay()
        {
            playerAnimator.Idle();
        }

        public void Action()
        {
            playerAnimator.Action();
        }

        public void StartDying()
        {
            playerInput.CanMove = false;
            destroyTimer = 0f;  
        }

        public void Destroy(GameTime p_GameTime)
        {
            if (playerAnimator.startDying)
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

        public void TouchedBy(GameObject p_By)
        {
            if (p_By is Ball)
            {
                //  Debug.WriteLine("PLAYER : J'ai touché la balle !");
            }
        }

    }
}
