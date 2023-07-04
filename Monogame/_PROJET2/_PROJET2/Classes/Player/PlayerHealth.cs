using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class PlayerHealth
    { 
        public float InitialLife { get; private set; }
        public float ProvisoryLife { get; private set; }
        public PlayerHealthUI playerHealthUI;

        private Player player;

        public bool IsDead { get; private set; }
        private bool criticalLifeAnnounced;
        private SoundEffect sndCriticalLife;
        public PlayerHealth(Player p_player, float initialLife) 
        {
        player = p_player;
        InitialLife = initialLife;
        ProvisoryLife = InitialLife;
        playerHealthUI = new PlayerHealthUI(this);
        PlayerState.SetLife((int)initialLife);
        sndCriticalLife = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>("Sounds/critical_life");
        }

        public void Update(GameTime p_gameTime)
        {
       
            if (PlayerState.Life != ProvisoryLife)
            {
                if (PlayerState.Life > 0)
                {
                    PlayerState.SubsLife(1);
                }
            }

            if (PlayerState.Life <= 0)
            {
                IsDead = true;
            }
            else if (PlayerState.Life <= InitialLife / 4)
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
                PlayerState.SetPoints(2950);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerHealthUI.Draw(spriteBatch);
        }

        public void Damage(float damage)
        {
            ProvisoryLife = PlayerState.Life - damage;
        }

    }
}
