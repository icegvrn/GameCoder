using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace BricksGame
{
    public class MonsterAnimator : Animator
    {
        private Monster monster;
        public bool startDying;
        private SoundManager soundContainer;
        private Color playerColor = Color.White;
        private float hitDuration = 1f;
        private float hitDurationTimer;
        private Texture2D attackIcon;

        public MonsterAnimator(Monster p_monster, float p_frameTime) : base(p_frameTime)
        {
            monster = p_monster;
            LoadTextures();
            ChangeState(Gamesystem.CharacterState.idle);
            soundContainer = new SoundManager(monster, monster.level);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
            attackIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/icon_ennemi");
        }

        public override void Update(GameTime gametime)
        {
            if (currentState == Gamesystem.CharacterState.hit)
            {
                hitDurationTimer -= (float)gametime.ElapsedGameTime.TotalSeconds;
                if (hitDurationTimer <= 0f)
                {
                    ChangeState(Gamesystem.CharacterState.idle);
                }
            }
            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            base.Draw(spriteBatch, position, playerColor);

            if (currentState == Gamesystem.CharacterState.fire)
            {
                spriteBatch.Draw(attackIcon, new Vector2((int)(monster.Position.X), (int)(monster.Position.Y - monster.monsterHeight / 2)), Color.White);
            }
        }

        public void Hit()
        {
            hitDurationTimer = hitDuration;
            ChangeState(Gamesystem.CharacterState.hit);
            soundContainer.Play(Gamesystem.CharacterState.hit, 1);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
        }

        public void Idle()
        {
        }

        public void Attack()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
        }

        public void AttackWithSound()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
                soundContainer.Play(Gamesystem.CharacterState.fire, 1);
        }

     

        public void Die()
        {
            ChangeState(Gamesystem.CharacterState.die);
            SetLoop(false);
            soundContainer.Play(Gamesystem.CharacterState.die, 1);
        }


        public void Reset()
        {
            playerColor = Color.White;
            ChangeState(Gamesystem.CharacterState.idle);
        }

        public void LoadTextures()
        {
            textureList = new List<Texture2D>();
            textureList.Insert((int)Gamesystem.CharacterState.idle, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/idle/" + monster.level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.l_idle, null);
            textureList.Insert((int)Gamesystem.CharacterState.walk, null);
            textureList.Insert((int)Gamesystem.CharacterState.l_walk, null);
            textureList.Insert((int)Gamesystem.CharacterState.fire, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/attack/" + monster.level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.l_fire, null);
            textureList.Insert((int)Gamesystem.CharacterState.hit, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/hit/" + monster.level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.die, ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/Monsters/die/" + monster.level + ""));

            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

    }
}
