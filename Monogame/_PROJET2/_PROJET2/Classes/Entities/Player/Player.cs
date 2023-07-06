using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// Classe contenant l'entité joueur en tant que telle. Le joueur est l'équivalent du pad classique d'un casse-brique. 
    /// </summary>
    public class Player : Sprite, ICollider
    {
        // Classes utilisées par le joueur
        private PlayerPowerManager playerPowerManager;
        private PlayerMouvement playerMouvement;
        private PlayerAnimator playerAnimator;
        private PlayerCollisionsManager playerCollisionsManager;
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

        // Caractéristiques
        public Vector2 Size { get; private set; }


        public Player(Texture2D p_texture) : base(p_texture)
        {
            InitComponents();
            CalculateSize();
            SetDefaultValues();
        }

        // Update des composants. On update la boundingBox joueur et on effectue les vérifications sur le joueur que si le joueur est en Ready.
        public override void Update(GameTime p_GameTime)
        {
            UpdateComponents(p_GameTime);

            if (IsReady)
            {
                UpdatePlayer(p_GameTime);
                UpdateBoundingBox();
            }
        }

        // Appel des draw des différents composants de joueur
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            playerAnimator.Draw(p_SpriteBatch, Position);
            playerHealth.Draw(p_SpriteBatch);
            playerPowerManager.Draw(p_SpriteBatch);
            playerFighter.Draw(p_SpriteBatch);
        }

        // Initialisation des composants de joueur : ses pouvoirs, son mouvement, ses animations, les input, sa vie, le composant de combat
        public void InitComponents()
        {
            playerPowerManager = new PlayerPowerManager(this);
            playerMouvement = new PlayerMouvement(this);
            playerCollisionsManager = new PlayerCollisionsManager(this);
            playerAnimator = new PlayerAnimator(this, 0.15f);
            playerInput = new PlayerInput(this);
            playerHealth = new PlayerHealth(this, 500f);
            playerFighter = new PlayerFighter(this);
        }

        // Valeurs par défaut attribuées au joueur au démarrage ; on reset le joueur et le met sur IDLE. 
        public void SetDefaultValues()
        {
            Speed = 15;
            Reset();
            ChangeState(Gamesystem.CharacterState.idle);
        }

        // Calcule de la taille d'une tile joueur (utilisé à plusieurs endroits du code)
        public void CalculateSize()
        {
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
        }

        // Méthode appelée pour changer l'état du joueur selon le CharacterState
        public void ChangeState(Gamesystem.CharacterState state)
        {
            CurrentState = state;
        }

        // Méthode d'Update permettant de vérifier si le joueur est mort, s'il est en train d'être blessé ( = animation à jouer dans le playerAnimator) ; update le powerManager.
        public void UpdatePlayer(GameTime p_GameTime)
        {
            if (playerHealth.IsDead)
            {
                Die(p_GameTime);
            }
            else
            {
                if (playerCollisionsManager.IsPlayerHit)
                {
                    playerAnimator.Blink(p_GameTime, true);
                }
                else
                {
                    playerAnimator.Blink(p_GameTime, false);
                }
              //  playerPowerManager.Update(p_GameTime);
            }
        }

        // Update de la boundingBox, modifiée pour avoir une collision proche d'un pad.
        public void UpdateBoundingBox()
        {
            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y / 3);
        }

        // Update des différents composants
        public void UpdateComponents(GameTime p_GameTime)
        {
            playerAnimator.Update(p_GameTime);
            playerInput.Update(p_GameTime);
            playerHealth.Update(p_GameTime);
            playerPowerManager.Update(p_GameTime);
        }

        // Reset de la position du joueur et de son animation
        public void Reset()
        {
            playerAnimator.Reset();
            Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - Size.X / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - Size.Y * 1.8f);
            playerFighter.Reset();
        }

        // Reset du joueur avec en plus le reset des munitions
        public void ResetAll()
        {
            IsReady = false;
            Reset();
            playerFighter.ResetMunition();
        }

        // Permet de passer le joueur à l'état de "prêt à jouer"
        public void SetReady()
        {
            if (!IsReady)
            {
                IsReady = true;
                Prepare();
            }
        }

        //Appelle la méthode Prepare du playerFighter qui crée une nouvelle balle prête à être lancée
        public void Prepare()
        {
            playerFighter.Prepare();
        }

        // Méthode appelée par le playerInput quand il va à gauche
        public void Left()
        {
            playerMouvement.Move(-1, 0);
            playerAnimator.ChangeDirection(Gamesystem.CharacterDirection.left);
        }

        // Méthode appelée par le playerInput quand il va à droite
        public void Right()
        {
            playerMouvement.Move(1, 0);
            playerAnimator.ChangeDirection(Gamesystem.CharacterDirection.right);
        }

        // Méthode appelée par le playerInput quand aucune touche gauche-droite n'est appuyée
        public void Stay()
        {
            playerAnimator.Idle();
        }

        // Méthode appelée par le playerInput quand la touche action est utilisée (pour le moment clic gauche de la souris). Actuellement, le joueur attaque quand on clic s'il n'est pas déjà en train d'attaquer.
        // S'il est déjà en train d'attaquer, on demande au powerManager de vérifier s'il y a un pouvoir à activer.
        public void Action()
        {
            playerAnimator.Action();

            if (!playerFighter.IsAttacker)
            {
                playerFighter.Attack();
                Stay();
            }
            else
            {
                playerPowerManager.ActivatePower();
            }
        }

        // Méthode appelée par le gameManager lorsqu'un monstre attaque le joueur (quand il est sur la dernière ligne jouable de la grille). Appelle la méthode OnHit.
        public void IsHit(ICollider h_By, int hitForce)
        {
            if (h_By is Monster)
            {
                OnHit(true);
                playerHealth.Damage(hitForce);
            }
        }

        // Méthode appelant à son tour la méthode OnHit du CollisionsManager.
        public void OnHit(bool b_hit)
        {
            playerCollisionsManager.OnHit(b_hit);
        }

        // Méthode appelée quand le joueur est mort et permettant de démarrer un timer pour jouer l'animation de mort.
        public void Die(GameTime p_GameTime)
        {
            playerAnimator.Die();
            Destroy(p_GameTime);
        }

        // Méthode appelée par l'animator quand le joueur est mort et permettant de démarrer un timer pour la méthode Destroy, laissant le temps à l'animator de jouer l'animation de mort.
        public void StartDying()
        {
            playerInput.CanMove = false;
            destroyTimer = 0f;
        }

        // Méthode permettant la destruction du joueur après un certain temps (permet de jouer l'animation de mort)
        public void Destroy(GameTime p_GameTime)
        {
            if (playerAnimator.StartDying)
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

        // Méthode TouchedBy relative aux gameObject. Non utilisée pour le moment.
        public void TouchedBy(GameObject p_By)
        {
        }
    }
}
