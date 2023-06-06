using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Bubble
{
    internal class BlueBubble : Bubble
    {
        public override void initImage(ContentManager content)
        {
            myBubbleSprite = content.Load<Texture2D>("bubble");
        }

        public override void initColor()
        {
            bubbleColor = Microsoft.Xna.Framework.Color.Blue;
        }

        public override void findNewDirection()
        {

            Random rand = new Random();
            int x = rand.Next(0, 2);
            int y = rand.Next(0, 2);

            if (x == 0)
            {
                x = -1;
            }

            if (y == 0)
            {
                y = -1;
            }

            myBubbleDirection = new Vector2(x, y);
        }


    }
}
