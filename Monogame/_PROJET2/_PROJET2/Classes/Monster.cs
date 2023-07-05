using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    public class Monster : Bricks, ICollider, IDestroyable
    {

        // Composants
        private MonsterHealth health;
        public MonsterFighter Fighter { get; set; }
        private MonsterAnimator animator;

        // Etat du monstre 
        public Gamesystem.CharacterState currentState;
        public bool startDying;
        public bool IsDead = false;

        private float destroyDelay = 0.8f;
        private float destroyTimer = 0f;

        // Caractéristique
        public int level;

        //Dimensions
        public int monsterWidth;
        public int monsterHeight;


        public Monster(Texture2D p_texture, int lvl) : base(p_texture)
        {
            InitDefaultValues(lvl);
            InitComponents(lvl);
            CalcMonsterDimensions();
            InitBoundingBox();
        }

        public override void Update(GameTime p_GameTime)
        {
            health.Update(p_GameTime);
            Fighter.Update(p_GameTime);
            animator.Update(p_GameTime);
            UpdateBoundingBox();
            CheckMonsterDying(p_GameTime);
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            animator.Draw(p_SpriteBatch, new Vector2((int)(Position.X - monsterWidth / 2), (int)(Position.Y - monsterHeight / 2)));
            health.Draw(p_SpriteBatch);
        }


        public void InitDefaultValues(int lvl)
        {
            level = lvl;
            SetSpeed(lvl);
            CanMove = true;
        }

        public void InitComponents(int lvl)
        {
            health = new MonsterHealth(this, lvl);
            Fighter = new MonsterFighter(this, lvl);
            animator = new MonsterAnimator(this, 0.15f);
        }

        public void CalcMonsterDimensions()
        {
            monsterHeight = currentTexture.Height;
            monsterWidth = currentTexture.Width / (currentTexture.Width / currentTexture.Height);
        }

        public void InitBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), monsterWidth, monsterHeight);
        }


        public void Attack()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                Fighter.StartAttack();
                animator.Attack();
                ChangeState(Gamesystem.CharacterState.fire);
            }

            else
            {
                Fighter.Attack();

                if (Fighter.Attacks)
                {
                    animator.AttackWithSound();
                }
            }
        }


        public void RemoveLife(float lifeFactor)
        {
            health.Damage(lifeFactor);
            ServiceLocator.GetService<ISessionService>().AddPoints((int)lifeFactor);
            CollisionEvent = false;
            animator.Hit();
        }


        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
            animator.ChangeState(state);
        }


        private void CheckMonsterDying(GameTime p_GameTime)
        {
            if (health.IsDead)
            {
                if (!IsDead)
                {
                    StartDying();
                }

                else
                {
                    UpdateDying(p_GameTime);
                }
            }

        }

        public void StartDying()
        {
            animator.Die();
            destroyTimer = 0f;
            IsDead = true;
        }

        public void UpdateDying(GameTime p_GameTime)
        {
            destroyTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            if (destroyTimer >= destroyDelay)
            {
                Destroy();
            }
        }

        private void UpdateBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X - monsterWidth / 2), (int)(Position.Y - currentTexture.Height / 2), monsterWidth, monsterHeight);
        }


        private void SetSpeed(int lvl)
        {
            switch ((lvl - 1) / 5)
            {
                case 0:
                    Speed = 2f;
                    break;
                case 1:
                    Speed = 2f;
                    break;
                case 2:
                    Speed = 2f;
                    break;
                case 3:
                    Speed = 1f;
                    break;

                default:
                    Speed = 1f;
                    break;
            }

        }

        public void Kill()
        {
            health.Kill();
        }

        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D boxTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(boxTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

    }


}



