using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BricksGame
{
    /// <summary>
    /// Classe représentant la balle : elle porte un PowerManager, un ParticlesManager, un hitManager, CollisionManager. La balle tape les monstres et leur enlève de la vie ou rebondit sur le pad (=joueur)
    /// </summary>
    public class Ball : Sprite, ICollider, IDestroyable
    {

        // Classes instanciées par Ball
        public BallCollisionManager CollisionManager { get; set; }
        public BallPowerManager PowerManager { get; set; }
        private MediaPlayerService soundContainer;
        private BallMovement ballMovement;
        private BallParticlesManager particlesManager;
        private BallHitManager hitManager;

        // Caractéristiques
        public int Strenght { get;  set; }       

        // Etats de la balle
        private Gamesystem.BallState currentSate;
        public bool IsFired { get; private set; }
        public bool IsDestroy { get; set; }


        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
            // Initialisation de la classe
            InitDefaultValues();
            InitComponents();
        }

        // Update permettant de mettre à jour les différentes instances de classe ; si la balle a été tirée on appelle le ballMovement pour la mettre en mouvement, sinon on la colle au pad
        public override void Update(GameTime p_GameTime)
        {
            if (IsFired)
            {
                ballMovement.SetMovable(true);
                particlesManager.AddParticles(p_GameTime); 
                ballMovement.TryMove();
            }
            else
            {
                UpdatePositionIfFollowingSomething();
            }

            hitManager.Update(p_GameTime);
            PowerManager.Update(p_GameTime);
            particlesManager.Update(p_GameTime);
            base.Update(p_GameTime);
        }

        // Draw de la balle : si elle n'a pas encore été tirée, on dessine un trait pour montrer où elle ira quand on va tirer. Appelle également le draw du PowerManager (utilisé pour le DamagePower qui a un compteur à côté de la balle)
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if ((!IsFired) && (ServiceLocator.GetService<LevelManager>().CurrentState == LevelManager.LevelState.play))
            {
                ballMovement.DrawTrajectory(p_SpriteBatch);
            }

            base.Draw(p_SpriteBatch);
            PowerManager.Draw(p_SpriteBatch);
        }

        // Initialisation des valeur par défaut de la balle
        public void InitDefaultValues()
        {
            Strenght = 50;
            currentSate = Gamesystem.BallState.idle;
        }

        // Instanciation des différentes classes utilisées par ball : le mouvement, les collisions, le pouvoir de balle, les particules, le dégats, les sons
        public void InitComponents()
        {
            soundContainer = new MediaPlayerService(this);
            ballMovement = new BallMovement(this, 10f);
            CollisionManager = new BallCollisionManager(this, ballMovement);
            PowerManager = new BallPowerManager(this);
            particlesManager = new BallParticlesManager(this);
            hitManager = new BallHitManager(this);
        }

        // Méthode fire qui permet de tirer la ball en demandant à ballMovement de calculer sa trajectoire. Lecture du son, on lui donne le droit de bouger et on la décolle du pad
        public void Fire()
        {
            if (!IsFired)
            {
                ObjectToFollow = null;
                CanMove = true;
                IsFired = true;
                ballMovement.CalcTrajectory();
                ChangeState(Gamesystem.BallState.fired);
                soundContainer.Play(Gamesystem.BallState.fired);
            }
        }

        // Méthode appelée quand il y a un hit avec quelque chose : elle appelle le hit de hitManager.
        private void HitEvents()
        {
            hitManager.HitEvents();  
        }

        // Méthode appelée à chaque fois que la balle change de direction
        public void OnDirectionChange()
        {
            HitEvents();
        }

        // Méthode permettant de changer la state de la balle (utilisé principalement pour jouer les bons sons)
        public void ChangeState(Gamesystem.BallState state)
        {
            currentSate = state;
        }

        // Méthode appelant l'activation du pouvoir en cours de PowerManager
        public void ActivePower()
        {
            PowerManager.ActivatePower();
        }

        // Méthode appelant l'activation d'un pouvoir défini dans PowerManager
        public void ActivatePower(Power power)
        {
            PowerManager.ActivatePower(power);
        
        }

        // Méthode appelant le déclanchement du pouvoir activé de PowerManager
        public void TriggerPower()
        {
            PowerManager.TriggerPower(); 
        }

        // Méthode indiquant au particlesManager de changer la couleur des particules quand un pouvoir est chargé
        public void OnPowerCharged()
        {
            particlesManager.PoweredColor(true);
        }
        // Méthode indiquant au particlesManager de remettre la couleur normale des particules quand un pouvoir a été déclanché
        public void OnPowerUsed()
        {
            particlesManager.PoweredColor(false);
        }

        // Méthode appelée quand il y a collision avec un monstre : appelle la méthode permettant de lui retirer de la vie, utilise le pouvoir si un pouvoir était activé
        public void OnCollide(Monster p_monster)
        {
            p_monster.RemoveLife(Strenght);

            if (PowerManager.IsPowerCharged())
            {
                PowerManager.TriggerPower(p_monster);
            }
        }

        // Methode appelée quand il y a collision avec un le joueur = pad. On joue un son
        public void OnCollide(Player p_player)
        {
            soundContainer.Play(Gamesystem.BallState.fired);
        }

        // Méthode appelée quand la balle sort de l'écran. Ici destruction.
         public void OutOfScreen()
        {
           Destroy(this);
        }

        // Méthode permettant de changer la vitesse de la balle (utilisée durant le pouvoir DoubleBall notamment)
        public void ChangeSpeed(float p_speedFactor)
        {
            ballMovement.SpeedVector *= p_speedFactor;
            Speed *= p_speedFactor;
        }


        // Méthode de destruction de la ball
        public void Destroy()
        {
            Destroy(this);
            soundContainer.Play(Gamesystem.BallState.destroyed);
        }

        // Méthode de destruction de la balle appelée par Destroy() : on vire les particules, on la retire de la liste des objets de scène, on indique son statut détruite.
        public void Destroy(IDestroyable ball)
        {
            particlesManager.DestroyAll();

            ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(this);
            currentSate = Gamesystem.BallState.destroyed;
            soundContainer.Play(Gamesystem.BallState.destroyed);
            IsDestroy = true;
            ball = null;
        }

        public void TouchedBy(GameObject p_By)
        {
            // Non utilisé
        }

    }



}