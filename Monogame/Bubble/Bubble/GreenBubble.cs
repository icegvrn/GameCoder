using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Bubble
{
    internal class GreenBubble : Bubble
    {


        public override void initImage(ContentManager content)
        {
            myBubbleSprite = content.Load<Texture2D>("bubble2");
        }

        public override void initColor()
        {
            bubbleColor = Microsoft.Xna.Framework.Color.White;
        }

        public override void findNewDirection()
        { 
            Random rand = new Random();
            int x = 0;
            int y = rand.Next(0, 2);

            if (y == 0)
            {
                y = -1;
            }

            myBubbleDirection = new Vector2(x, y);
        }

    }
}
