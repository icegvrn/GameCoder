using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Classe héritante d'Animator qui permet d'animer un monstre. Elle contient les textures mais aussi le comportement particulier du monstre : animation d'attaque et de hit
    /// </summary>
    public class MonsterAnimator : Animator
    {
        private Monster monster;
        private MediaPlayerService soundContainer;
        private Color playerColor = Color.White;
        private float hitDuration = 1f;
        private float hitDurationTimer;
        private Texture2D attackIcon;

        public MonsterAnimator(Monster p_monster, float p_frameTime) : base(p_frameTime)
        {
            monster = p_monster;
            LoadTextures();
            ChangeState(Gamesystem.CharacterState.idle);
            soundContainer = new MediaPlayerService(monster, monster.Level);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
            attackIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetMonstersImagesPathRoot()+"icon_ennemi");
        }

        // Update : si le monstre est hit, il le reste le temps de lire l'animation, puis repasse en IDLE
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

        // Dessin du monstre : si le monstre est attaquant, ajout d'une icone le signifiant pour le joueur
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            base.Draw(spriteBatch, position, playerColor);

            if (currentState == Gamesystem.CharacterState.fire)
            {
                spriteBatch.Draw(attackIcon, new Vector2((int)(monster.Position.X), (int)(monster.Position.Y - monster.MonsterHeight / 2)), Color.White);
            }
        }

        // Méthode permettant de jouer l'animation hit pendant un temps donné avant de repasser en IDLE (update) avec lecture du son
        public void Hit()
        {
            hitDurationTimer = hitDuration;
            ChangeState(Gamesystem.CharacterState.hit);
            soundContainer.Play(Gamesystem.CharacterState.hit, 1);
            soundContainer.Play(Gamesystem.CharacterState.idle, 1);
        }

        // Méthode permettant de jouer l'animation attack du monstre
        public void Attack()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
        }

        // Méthode permettant de jouer l'animation attack du monstre en y ajoutant un son
        public void AttackWithSound()
        {
            if (currentState != Gamesystem.CharacterState.fire)
            {
                ChangeState(Gamesystem.CharacterState.fire);
            }
                soundContainer.Play(Gamesystem.CharacterState.fire, 1);
        }

        // Méthode permettant de jouer l'animation die du monstre
        public void Die()
        {
            ChangeState(Gamesystem.CharacterState.die);
            SetLoop(false);
            soundContainer.Play(Gamesystem.CharacterState.die, 1);
        }

        // Reset de l'animation du monstre : on repasse en IDLE et en couleur standard.
        public void Reset()
        {
            playerColor = Color.White;
            ChangeState(Gamesystem.CharacterState.idle);
        }

        // Chargement des textures pour le monstre
        public void LoadTextures()
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            string imgPath = ServiceLocator.GetService<IPathsService>().GetMonstersImagesPathRoot();
            textureList = new List<Texture2D>();
            textureList.Insert((int)Gamesystem.CharacterState.idle, content.Load<Texture2D>(imgPath+"idle/" + monster.Level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.l_idle, null);
            textureList.Insert((int)Gamesystem.CharacterState.walk, null);
            textureList.Insert((int)Gamesystem.CharacterState.l_walk, null);
            textureList.Insert((int)Gamesystem.CharacterState.fire, content.Load<Texture2D>(imgPath + "attack/" + monster.Level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.l_fire, null);
            textureList.Insert((int)Gamesystem.CharacterState.hit, content.Load<Texture2D>(imgPath+ "hit/" + monster.Level + ""));
            textureList.Insert((int)Gamesystem.CharacterState.die, content.Load<Texture2D>(imgPath+"die/" + monster.Level + ""));

            ChangeSpriteSheet(textureList[(int)Gamesystem.CharacterState.idle]);
        }

    }
}
