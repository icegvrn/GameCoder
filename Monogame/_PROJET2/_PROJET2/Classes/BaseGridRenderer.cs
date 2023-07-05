using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BricksGame
{
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

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gridTexture, gridTexturePos, Color.White);
        }

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

        private void DrawGridRectangle(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.White);
        }
    }
}
