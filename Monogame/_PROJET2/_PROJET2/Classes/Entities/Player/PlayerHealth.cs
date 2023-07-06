using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BricksGame
{
    /// <summary>
    /// Classe qui gère le niveau de vie du joueur
    /// </summary>
    public class PlayerHealth : Health
    {
        private Player player;
        private PlayerHealthUI playerHealthUI;

        //Etats
        private ISessionService playerState;

        // Son
        private bool criticalLifeAnnounced;
        private SoundEffect sndCriticalLife;

        public PlayerHealth(Player p_player, float initialLife)
        {
            player = p_player;
            playerState = ServiceLocator.GetService<ISessionService>();
            InitLife(initialLife);
            playerHealthUI = new PlayerHealthUI(this);
            sndCriticalLife = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(ServiceLocator.GetService<IPathsService>().GetSoundsRoot() + "critical_life");
        }

        //Update de la vie : animation progressive de la vie qui descend si le joueur a été touché, mise à mort s'il est à 0 de points de vie et annonce sonore s'il approche de la fin de vie
        public override void Update(GameTime p_gameTime)
        {
            if (playerState.GetLife() != ProvisoryLife)
            {
                if (playerState.GetLife() > 0)
                {
                    playerState.SubsLife(1);
                }
            }

            if (playerState.GetLife() <= 0)
            {
                IsDead = true;
            }
            else if (playerState.GetLife() <= InitialLife / 4)
            {
                if (!criticalLifeAnnounced)
                {
                    sndCriticalLife.Play();
                    criticalLifeAnnounced = true;
                }
            }

            playerHealthUI.Update(p_gameTime);

            DebugInputEnable();

        }

        // Dessin de la barre de vie en appelant le draw de playerHealthUI
        public override void Draw(SpriteBatch spriteBatch)
        {
            playerHealthUI.Draw(spriteBatch);
        }

        //Initialisation de la vie du joueur
        public override void InitLife(float initialLife)
        {
            InitialLife = initialLife;
            ProvisoryLife = InitialLife;
            playerState.SetLife((int)initialLife);
        }

        // Méthode permettant de mettre des dégâts au joueur
        public override void Damage(float damage)
        {
            ProvisoryLife = playerState.GetLife() - damage;
        }

   

        // Pour le debug : méthode qui permet d'augmenter presque au maximum les points gagnés 
        private void DebugInputEnable()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                playerState.SetPoints(2950);
            }
        }

    }
}
