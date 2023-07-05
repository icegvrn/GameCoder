using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    public class Ball : Sprite, ICollider, IDestroyable
    {
        public int Strenght { get;  set; }       

        // Etats de la balle
       private Gamesystem.BallState currentSate;
        public bool isFired;
        public bool IsDestroy { get; set; }

        // Composants
        public BallCollisionManager CollisionManager { get; set; }
        public BallPowerManager PowerManager { get; set; }
        private MediaPlayerService soundContainer;
        private BallMovement ballMovement;
        private BallParticlesManager particlesManager;
        private BallHitManager hitManager;

        public Ball(List<Texture2D> p_texture) : base(p_texture)
        {
           Strenght = 50; 
           currentSate = Gamesystem.BallState.idle;
            soundContainer = new MediaPlayerService(this);
            ballMovement = new BallMovement(this, 10f);
            CollisionManager = new BallCollisionManager(this, ballMovement);
            PowerManager = new BallPowerManager(this);
            particlesManager = new BallParticlesManager(this);
            hitManager = new BallHitManager(this);
           

        }

        public override void Update(GameTime p_GameTime)
        {
            CollisionManager.Update(p_GameTime);

            if (isFired)
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

        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if ((!isFired) && (ServiceLocator.GetService<LevelManager>().CurrentState == LevelManager.LevelState.play))
            {
                ballMovement.DrawTrajectory(p_SpriteBatch);
            }

            base.Draw(p_SpriteBatch);
            PowerManager.Draw(p_SpriteBatch);
        }

        public void Fire()
        {
            if (!isFired)
            {
                ObjectToFollow = null;
                ballMovement.CalcTrajectory();
                CanMove = true;
                isFired = true;
                ChangeState(Gamesystem.BallState.fired);
                soundContainer.Play(Gamesystem.BallState.fired);
            }

        }

        private void HitEvents()
        {
            hitManager.HitEvents();
           
        }

      

        public void Destroy(IDestroyable ball)
        {
            particlesManager.DestroyAll();
        
            ServiceLocator.GetService<GameState>().CurrentScene.RemoveToGameObjectsList(this);
            currentSate = Gamesystem.BallState.destroyed;
            soundContainer.Play(Gamesystem.BallState.destroyed);
            IsDestroy = true;
            ball = null;
        }

        public void Destroy()
        {
            Destroy(this);
            soundContainer.Play(Gamesystem.BallState.destroyed);
        }

        public void ChangeState(Gamesystem.BallState state)
        {
            currentSate = state;
        }


        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D blankTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
        }

        public void ActivePower()
        {
            PowerManager.ActivatePower();
        }

        public void ActivatePower(Power power)
        {
            PowerManager.ActivatePower(power);
        
        }

        public void TriggerPower()
        {
            PowerManager.TriggerPower(); 
        }

        public void OnDirectionChange()
        {
            HitEvents();
        }

        public void OnCollide(Monster p_monster)
        {
            p_monster.RemoveLife(Strenght);

            if (PowerManager.IsPowerCharged())
            {
                PowerManager.TriggerPower(p_monster);
            }
        }

        public void OnCollide(Player p_player)
        {
            soundContainer.Play(Gamesystem.BallState.fired);
        }

    public void OutOfScreen()
        {
           Destroy(this);
        }

        public void ChangeSpeed(float p_speedFactor)
        {
            ballMovement.SpeedVector *= p_speedFactor;
            Speed *= p_speedFactor;
        }

       public void OnPowerCharged()
        {
            particlesManager.PoweredColor(true);
        }
        public void OnPowerUsed()
        {
            particlesManager.PoweredColor(false);
        }


        public void TouchedBy(GameObject p_By)
        {
            //
        }

    }



}