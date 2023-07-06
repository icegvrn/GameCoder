using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Classe qui gère la dimension "combat" du joueur en lui permettant d'avoir des munitions et de tirer des balles. 
    /// </summary>
    public class PlayerFighter : Fighter
    {
        private Player player;

        // Projectiles du joueur
        public List<Ball> BallsList { get; private set; }
      
        // Etats des munitions
        private int munitionNb;
        private int baseMunition;
        public bool HasMunition { get { if (munitionNb <= 0) { return false; } else { return true; } } }

        // UI portrait du combattant et munitions
        private string imgPath;
        private Texture2D munitionCounter;
        private Texture2D portrait;


        public PlayerFighter(Player p_player) {

            player = p_player;
            InitFighter();


            // Portrait du joueur affiché au niveau des munitions
            imgPath = ServiceLocator.GetService<IPathsService>().GetImagesRoot();
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "character_portrait");
            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "icon_counter_true");
         

        }

        //Dessin du portrait combattant et des munitions restantes
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw portrait combattant
            Vector2 portraitPosition = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - portrait.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - portrait.Height * 1.25f);
            spriteBatch.Draw(portrait, portraitPosition, Color.White);

            //Draw munitions counter 
            for (int n = 0; n < munitionNb; n++)
            {
                spriteBatch.Draw(munitionCounter, new Vector2(portraitPosition.X + 6 * (n), portraitPosition.Y + portrait.Height + 2), Color.White);
            }
        }

        // Initialisation des informations de combat : munition restantes, attaquant ou non etc.
        private void InitFighter()
        {
            BallsList = new List<Ball>();
            munitionNb = 10;
            baseMunition = 10;
            IsAttacker = false;
        }

        // Méthode attack du combattant, permettant d'appeler la méthode fire de la ball si toutes les conditions sont réunies. 
        public override void Attack()
        {
            if (!IsAttacker && HasMunition)
            {
                foreach (Ball ball in BallsList)
                {
                    if (!ball.IsFired)
                    {
                        StartAttack(); 
                        ball.Fire();  
                        return;
                    }
                }  
            }
        }

        // Enlève une munition et passe le joueur en Attacker quand on vient de démarrer une attaque
        public override void StartAttack()
        {
            RemoveMunition(1);
            IsAttacker = true;
            player.Stay();
        }


        // Méthode prépare permet de créer une nouvelle balle et de repasser le combattant en mode pas attacker tant qu'il a pas relancé une attaque
        public void Prepare()
        {
            CreateNewBall();
            IsAttacker = false;
        }

        // Méthode permettant de créer une nouvelle balle et de l'ajouter au jeu. 
        public Ball CreateNewBall()
        {
            List<Texture2D> ballTextureList = new List<Texture2D>();
            ballTextureList.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>(imgPath + "ball"));
            Ball ball = new Ball(ballTextureList);
            ball.Position = new Vector2(player.Position.X + player.Size.X / 2, player.Position.Y - 20f);
            // Colle la balle au pad
            ball.Following(player);
            // L'ajoute à la scène
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(ball);
            BallsList.Add(ball);
            return ball;
        }


        // Méthode permettant de tirer une balle "secondaire", c'est à dire avec moins de vitesse qu'une balle classique
        public void FireASecondaryBall()
        {
            BallsList[1].ChangeSpeed(0.5f);
            BallsList[1].Fire();

        }

        // Méthode permettant de reset les munitions, appelés quand on change de niveau
        public void ResetMunition()
        {
            munitionNb = baseMunition;
        }

        // Méthode permettant de remove une munition, appelé quand le joueur fait une attaque
        public void RemoveMunition(int nb)
        {
            munitionNb -= nb;
        }

        // Méthode faisant un reset de tous les projectiles en cours ; utilisé quand on change de niveau ou quand le joueur perd/gagne
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

        // Méthode permettant de transmettre à la ball l'activation d'un pouvoir
        public void ActivatePowerOnBall(Power power)
        {
            BallsList[0].ActivatePower(power); 
        }

        // Méthode permettant de transmettre à la ball le trigger d'un pouvoir
        public void TriggerPower()
        {
            BallsList[0].TriggerPower();
        }


           public override void Update(GameTime gameTime)
        {

        }

    }
}

