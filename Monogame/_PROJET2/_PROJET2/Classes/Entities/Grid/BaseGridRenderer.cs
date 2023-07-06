using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BricksGame
{
    /// <summary>
    /// Classe qui gère l'affichage de la grille de jeu. Gère la grille et aussi l'affichage des slots en mode debug ou pour le mode editor
    /// </summary>
    public class BaseGridRenderer
    {
        private List<Rectangle> slotRectangles;
        public List<Rectangle> SlotRectangles { get { return slotRectangles; } }
        private Texture2D blankTexture;

        private Texture2D gridTexture;
        private Vector2 gridTexturePos;

        public BaseGridRenderer(List<Vector2> p_VectorList, int brickWidth, int brickHeight)
        {
            InitGrid();
            InitSlots(p_VectorList, brickWidth, brickHeight);
        }

        public void InitGrid()
        {
            gridTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "grid");
            gridTexturePos = new Vector2(0, 9);
        }

        // Initialisation des différents slots de la grille qui deviennent des rectangles pour pouvoir être affichés
        public void InitSlots(List<Vector2> p_VectorList, int brickWidth, int brickHeight)
        {
            slotRectangles = new List<Rectangle>();
            foreach (Vector2 vector in p_VectorList)
            {
                Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, brickWidth, brickHeight);
                slotRectangles.Add(rectangle);
            }
            blankTexture = ServiceLocator.GetService<IAssetsServices>().GetGameTexture(IAssetsServices.textures.blank);
        }

        // Dessin de la grille visuelle
        public void DrawGrid(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gridTexture, gridTexturePos, Color.White);
        }

        // Dessin des slots réels en tenant compte de la volonté d'afficher les grilles où les monstres n'ont pas le droit d'aller ou non
        public void DrawSlots(SpriteBatch spriteBatch, bool showOutOfLimitSlot, int maxDestination)
        {
            foreach (Rectangle rect in slotRectangles)
            {
                if (!showOutOfLimitSlot)
                {
                    if (rect.Top <= maxDestination)
                    {
                        DrawGridRectangle(spriteBatch, rect);
                    }
                }
                else
                {
                    DrawGridRectangle(spriteBatch, rect);
                }
            }
        }

        //Dessin d'un rectangle, appelé pour chaque slot dans DrawSlots
        private void DrawGridRectangle(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.White);
        }
    }
}
