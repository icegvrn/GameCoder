using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;



namespace Bubble
{
    internal class BubbleList
    {
        private List<Bubble> bubbleList;

        public void initBubbles(int blueBubble, int redBubble, int greenBubble)
        {

           GraphicsDevice graphics = ServiceLocator.GetService<GraphicsDevice>();
           ContentManager content = ServiceLocator.GetService<ContentManager>();

            bubbleList = new List<Bubble>();

                Random rand = new Random();
                int width = graphics.Viewport.Width;
                int height = graphics.Viewport.Height;

            for (int i=0; i< blueBubble; i++)
            {
                BlueBubble newBubble = new BlueBubble();  
                bubbleList.Add(newBubble);
            }

            for (int i = 0; i < redBubble; i++)
            {
                RedBubble newBubble = new RedBubble();  
                bubbleList.Add(newBubble);
            }

            for (int i = 0; i < greenBubble; i++)
            {
                GreenBubble newBubble = new GreenBubble();
                bubbleList.Add(newBubble);
            }

            foreach (Bubble bubble in bubbleList)
            {
                bubble.initBubble(content);
                bubble.setPosition(rand.Next(0, width - bubble.getBubbleWidth()), rand.Next(0, height - bubble.getBubbleHeight()));
            }
        }

        public void DrawAllBubble(SpriteBatch spriteBatch)
        {
            foreach (Bubble bubble in bubbleList)
            {
                bubble.DrawBubble(spriteBatch);
            }
        }

        public void MoveAllBubble()
        {
         
            foreach (Bubble bubble in bubbleList)
            {
                bubble.moveBubble();
            }
        }
    }
}
