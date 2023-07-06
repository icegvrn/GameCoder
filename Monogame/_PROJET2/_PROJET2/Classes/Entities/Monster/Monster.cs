using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// Classe de bricks "Monstre" qui a la particularité d'avoir un animator, un fighter et de la vie. 
    /// </summary>
    public class Monster : Bricks
    {
        // Classes instanciées par Monster
        private MonsterHealth health;
        private MonsterAnimator animator;
        public MonsterFighter Fighter { get; set; }


        // Etat du monstre 
        private float destroyDelay = 0.8f;
        private float destroyTimer = 0f;
        public Gamesystem.CharacterState currentState;
        public bool startDying { get; set; }
        public bool IsDead { get; set; }

        // Caractéristique
        public int Level { get; private set; }

        //Dimensions
        public int MonsterWidth { get; private set; }
        public int MonsterHeight { get; private set; }


        public Monster(Texture2D p_texture, int lvl) : base(p_texture)
        {
            // Initialisation des classes et calcul de la taille de la tile
            InitDefaultValues(lvl);
            InitComponents(lvl);
            CalcMonsterDimensions();
        }

        //Update des classes servant de composants 
        public override void Update(GameTime p_GameTime)
        {
            health.Update(p_GameTime);
            Fighter.Update(p_GameTime);
            animator.Update(p_GameTime);
            UpdateBoundingBox();
            CheckMonsterDying(p_GameTime);
        }

        // Dessin de l'animator et de la vie
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            animator.Draw(p_SpriteBatch, new Vector2((int)(Position.X - MonsterWidth / 2), (int)(Position.Y - MonsterHeight / 2)));
            health.Draw(p_SpriteBatch);
        }

        // Initialisation des valeurs par défaut à l'instanciation : basées sur le niveau du monstre (= la valeur du dé)
        public void InitDefaultValues(int lvl)
        {
            Level = lvl;
            SetSpeed(lvl);
            CanMove = true;
            IsDead = false;
    }

        // Initialisation des classes utilisées : la vie, le composant de combat et l'animator
        public void InitComponents(int lvl)
        {
            health = new MonsterHealth(this, lvl);
            Fighter = new MonsterFighter(this, lvl);
            animator = new MonsterAnimator(this, 0.15f);
        }

        // Calcul de la taille de la tile, utilisé à plusieurs endroit du code
        public void CalcMonsterDimensions()
        {
            MonsterHeight = currentTexture.Height;
            MonsterWidth = currentTexture.Width / (currentTexture.Width / currentTexture.Height);
        }

        // Update de la BoundingBox du monstre
        private void UpdateBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X - MonsterWidth / 2), (int)(Position.Y - currentTexture.Height / 2), MonsterWidth, MonsterHeight);
        }

        // Méthode Attack : appelle les méthodes liées à l'attaque du Fighter et de l'animator ; change l'état du monstre à fire (=attack)
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

        // Méthode appelée quand le monstre est touché par la balle. Indique au monsterHealth de prendre des damages et à l'animator de jouer les animations de hit.
        public void RemoveLife(float lifeFactor)
        {
            health.Damage(lifeFactor);
            ServiceLocator.GetService<ISessionService>().AddPoints((int)lifeFactor);
            animator.Hit();
        }

        // Permet de changer l'état du monstre et transmet l'information à l'animator
        public void ChangeState(Gamesystem.CharacterState state)
        {
            currentState = state;
            animator.ChangeState(state);
        }

        // Méthode attribuant une vitesse au monstre en fonction de son niveau,  (utilisée par le gridAnimator de baseGrid pour descendre à différente vitesse sur la grille). 
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

        // Méthode d'update vérifiant si le monsterHealth est à l'état de IsDead, si oui on démarre l'animation de mort si le monstre n'était pas déjà indiqué comme mourrant, sinon on continue l'animation
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

        // Méthode permettant de démarrer l'animation de mort : lance l'animation sur l'animator en changeant l'état, prépare un timer pour Destroy afin de laisser le temps à l'animation de mort de se jouer
        public void StartDying()
        {
            animator.Die();
            destroyTimer = 0f;
            IsDead = true;
        }

        // Mise à jour du timer de mort avant de destroy pour laisser le temps à l'animation de mort de se jouer
        public void UpdateDying(GameTime p_GameTime)
        {
            destroyTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
            if (destroyTimer >= destroyDelay)
            {
                Destroy();
            }
        }

        // Méthode permettant de tuer directement un monstre en appelant la méthode Kill sur Health (met la vie à 0)
        public void Kill()
        {
            health.Kill();
        }

    }


}



