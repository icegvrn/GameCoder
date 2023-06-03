using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble
{
    internal class RedBubble : Bubble
    {

        public override void initImage(ContentManager content)
        {
            myBubbleSprite = content.Load<Texture2D>("bubble3");
        }

        public override void initColor()
        {
            bubbleColor = Microsoft.Xna.Framework.Color.Pink;
        }

        public override void findNewDirection()
        {
            Random rand = new Random();
            int x = rand.Next(0, 2);
            int y = 0;

            if (x == 0)
            {
                x = -1;
            }
            myBubbleDirection = new Vector2(x, y);
        }

    }
}
