using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace BricksGame
{
    public delegate void OnClick(Button p_Sender);
    public delegate void OnHover(Button p_Sender);

    /// <summary>
    /// Classe boutton, permet de créer un nouveau bouton à partir d'une texture et d'avoir les fonctionnalités hover,click etc. Les méthodes au clic sont déléguées
    /// </summary>
    public class Button : Sprite
    {
        public bool isHover { get; private set; }

        public OnClick onClick { get; set; }
        public OnHover onHover { get; set; }

        public Button(List<Texture2D> p_texture) : base(p_texture)
        {
        }

        //A l'update on vérifie s'il y a hover en regardant si la souris est dans le BoundingBox du boutton et on vérifie s'il y a un clic souris au même moment pour déterminer si le joueur a cliqué
        public override void Update(GameTime gameTime)
        {
            if (BoundingBox.Contains(ServiceLocator.GetService<IInputService>().GetMousePosition()))
            {
                if (!isHover)
                {
                    isHover = true;
                    changeSprite(1);
                    onHover(this);
                }
                else
                {
                    if (ServiceLocator.GetService<IInputService>().OnActionReleased())
                    {
                        if (onClick != null)
                        {
                            onClick(this);
                        }
                    }
                }
            }
            else
            {
                isHover = false;
                changeSprite(0);
            }

            base.Update(gameTime);
        }

        // Méthode permettant de changer de sprite : utilisé au hover sur le boutton
        private void changeSprite(int nb)
        {
            if (nb <= Textures.Count() - 1)
            {
                if (Textures[nb] != null)
                {
                    currentTexture = Textures[nb];
                }
            }
        }

        // Méthode utilisé pour modifier le spriteEffect du bouton, est utilisé pour les effets miroir
        public void ChangeSpriteEffects(SpriteEffects sFX)
        {
            spriteEffects = sFX;
        }
    }
}
