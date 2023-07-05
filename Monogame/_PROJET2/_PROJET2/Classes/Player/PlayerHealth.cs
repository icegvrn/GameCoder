using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BricksGame
{
    public class PlayerHealth : Health
    { 
        public PlayerHealthUI playerHealthUI;

        private Player player;
        private ISessionService playerState;

        private bool criticalLifeAnnounced;
        private SoundEffect sndCriticalLife;

        public PlayerHealth(Player p_player, float initialLife) 
        {
        playerState = ServiceLocator.GetService<ISessionService>();
        player = p_player;
        InitLife(initialLife);
        playerHealthUI = new PlayerHealthUI(this);
        sndCriticalLife = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(ServiceLocator.GetService<IPathsService>().GetSoundsRoot()+"critical_life");

        }

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

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                playerState.SetPoints(2950);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerHealthUI.Draw(spriteBatch);
        }

        public override void Damage(float damage)
        {
            ProvisoryLife = playerState.GetLife() - damage;
        }

        public override void InitLife(float initialLife)
        {
            InitialLife = initialLife;
            ProvisoryLife = InitialLife;
            playerState.SetLife((int)initialLife);
        }

    }
}
