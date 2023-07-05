using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    public class PlayerFighter : Fighter
    {
        private Player player;
        private List<Ball> balls;
        public List<Ball> BallsList { get { return balls; } private set { balls = value; } }
        private int munitionNb;
        private int baseMunition;
       
        public bool HasMunition { get { if (munitionNb <= 0) { return false; } else { return true; } } }

        private Texture2D munitionCounter;
        private Texture2D portrait;

        private string imgPath;

        public PlayerFighter(Player p_player) {

            player = p_player;
            balls = new List<Ball>();
            munitionNb = 10;
            baseMunition = 10;
            IsAttacker = false;
            imgPath = ServiceLocator.GetService<IPathsService>().GetImagesRoot();
            // Portrait
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "character_portrait");

            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "icon_counter_true");
            BallsList = new List<Ball>();

        }

     
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw portrait
            Vector2 portraitPosition = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - portrait.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - portrait.Height * 1.25f);
            spriteBatch.Draw(portrait, portraitPosition, Color.White);

            //Draw Counter 
            for (int n = 0; n < munitionNb; n++)
            {
                spriteBatch.Draw(munitionCounter, new Vector2(portraitPosition.X + 6 * (n), portraitPosition.Y + portrait.Height + 2), Color.White);
            }
        }


        public override void Attack()
        {
            if (!IsAttacker && HasMunition)
            {
                foreach (Ball ball in BallsList)
                {
                    if (!ball.isFired)
                    {
                        StartAttack(); 
                        ball.Fire();
                                
                        return;
                    }
                }
               
            }
        }

        public override void StartAttack()
        {
            RemoveMunition(1);
            IsAttacker = true;
            player.Stay();
        }

        public Ball CreateNewBall()
        {
            List<Texture2D> ballTextureList = new List<Texture2D>();
            ballTextureList.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "ball"));
            Ball ball = new Ball(ballTextureList);
            ball.Position = new Vector2(player.Position.X + player.Size.X / 2, player.Position.Y - 20f);
            ball.Following(player);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(ball);
            BallsList.Add(ball);
            return ball;
        }

        public void Prepare()
        {
            CreateNewBall();
            IsAttacker = false;
        }

        public void ResetMunition()
        {
            munitionNb = baseMunition;
        }

        public void RemoveMunition(int nb)
        {
            munitionNb -= nb;
        }

        public void Reset()
        {
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

        public void ActivatePowerOnBall(Power power)
        {
            BallsList[0].ActivatePower(power); 
        }

        public void TriggerPower()
        {
            BallsList[0].TriggerPower();
        }

        public void FireASecondaryBall()
        {
            BallsList[1].ChangeSpeed(0.5f);
            BallsList[1].Fire();

        }

           public override void Update(GameTime gameTime)
        {

        }

    }
}

