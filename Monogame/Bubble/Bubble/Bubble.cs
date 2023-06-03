using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Bubble
{
    internal class Bubble
    {
        protected Texture2D myBubbleSprite;
        protected Vector2 myBubblePosition;
        protected Vector2 myBubbleDirection;
        protected Microsoft.Xna.Framework.Color bubbleColor;

        public void initBubble(ContentManager content)
        {
            initImage(content);
            initColor();
            findNewDirection();     
        }

        public void setPosition(int p_x, int p_y)
        {
            myBubblePosition = new Vector2(p_x, p_y);
        }

        public void DrawBubble(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(myBubbleSprite, myBubblePosition, bubbleColor);
        }

        public void moveBubble(GraphicsDevice graphics)
        {
            if (myBubblePosition.X + myBubbleDirection.X < 0 || myBubblePosition.X + myBubbleDirection.X + myBubbleSprite.Width > graphics.Viewport.Width || myBubblePosition.Y + myBubbleDirection.Y < 0 || myBubblePosition.Y + myBubbleDirection.Y + myBubbleSprite.Height > graphics.Viewport.Height)
            {
                findNewDirection();
            }
            else
            {
                myBubblePosition = new Vector2((myBubblePosition.X + myBubbleDirection.X), (myBubblePosition.Y + myBubbleDirection.Y));       
            }
        }

        public virtual void initImage(ContentManager content)
        {
            myBubbleSprite = content.Load<Texture2D>("bubble");
        }

        public virtual void initColor()
        {
            bubbleColor = Microsoft.Xna.Framework.Color.White;
        }

        public virtual void findNewDirection()
        {
            Random rand = new Random();
            int x = rand.Next(0, 2);
            int y = rand.Next(0, 2);

            if (x == 0)
            {
                x = -1 ;
            }

            if (y == 0)
            {
                y = -1 ;
            }

            myBubbleDirection = new Vector2(x, y);
        }


        public int getBubbleHeight()
        {
            return myBubbleSprite.Height;
        }

        public int getBubbleWidth()
        {
            return myBubbleSprite.Width;
        }
    }
}
