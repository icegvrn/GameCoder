using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    public class Player : Sprite, ICollider
    {
        // Composants
        private PlayerPowerManager playerPowerManager;
        private PlayerMouvement playerMouvement;
        private PlayerAnimator playerAnimator;
        private PlayerInput playerInput;
        private PlayerHealth playerHealth;
        public PlayerFighter playerFighter;

        // Etat du joueur 
        public bool IsDead = false;
        public bool IsReady = false;
        public Gamesystem.CharacterState CurrentState { get; private set; }

        //Destruction du joueur
        private float destroyDelay = 2f;
        private float destroyTimer = 0f;

        // Collisions
        public bool IsCollide { private set; get; }
        public bool IsPlayerHit { get; set; }
        public Vector2 Size { get; private set; }


        public Player(Texture2D p_texture) : base(p_texture)
        {
            InitComponents();
           CalculateSize();
            SetDefaultValues();
        }

        public override void Update(GameTime p_GameTime)
        {
            UpdateComponents(p_GameTime);

            if (IsReady)
            {
                UpdatePlayer(p_GameTime);
                UpdateBoundingBox();
            }
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            playerAnimator.Draw(p_SpriteBatch, Position);
            playerHealth.Draw(p_SpriteBatch);
            playerPowerManager.Draw(p_SpriteBatch);
            playerFighter.Draw(p_SpriteBatch);
        }


        public void InitComponents()
        {
            playerPowerManager = new PlayerPowerManager(this);
            playerMouvement = new PlayerMouvement(this);
            playerAnimator = new PlayerAnimator(this, 0.15f);
            playerInput = new PlayerInput(this);
            playerHealth = new PlayerHealth(this, 500f);
            playerFighter = new PlayerFighter(this);
        }

        public void CalculateSize()
        {
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
        }

        public void SetDefaultValues()
        {
            Speed = 15;
            Reset();
            ChangeState(Gamesystem.CharacterState.idle);
        }

        public void ChangeState(Gamesystem.CharacterState state)
        {
            CurrentState = state;

        }

        public void UpdatePlayer(GameTime p_GameTime)
        {
            if (playerHealth.IsDead)
            {
                Die(p_GameTime);
            }
            else
            {
                playerPowerManager.Update(p_GameTime);

                if (IsPlayerHit)
                {
                    playerAnimator.Blink(p_GameTime, true);
                }

                else
                {
                    playerAnimator.Blink(p_GameTime, false);
                }
            }
        }

        public void UpdateBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y / 3);
        }

        public void UpdateComponents(GameTime p_GameTime)
        {
            playerAnimator.Update(p_GameTime);
            playerInput.Update(p_GameTime);
            playerHealth.Update(p_GameTime);
            playerPowerManager.Update(p_GameTime);
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
            playerFighter.Reset();

        }


        public void Prepare()
        {
            playerFighter.Prepare();
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

            if (!playerFighter.hasFired)
            {
                playerFighter.Fire();
                Stay();
            }
            else
            {
                playerPowerManager.ActivatePower();
            }

        }


        public void IsHit(ICollider h_By, int hitForce)
        {
            if (h_By is Monster)
            {
                IsPlayerHit = true;
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

        public void TouchedBy(GameObject p_By)
        {
            if (p_By is Ball)
            {
            }
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

        public void Die(GameTime p_GameTime)
        {
            playerAnimator.Die();
            Destroy(p_GameTime);
        }


    }
}
