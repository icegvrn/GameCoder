using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class BricksList
    {
        private List<Bricks> listOfBricks;
        public List<Bricks> ListOfBricks { get { return listOfBricks; } set { listOfBricks = value; } }

        private float bricksMargin = 30f;
        private float downSpeed = 15f;

        public void CreateBricksWall()
        {
            ListOfBricks = new List<Bricks>();
            List<Texture2D> list = new List<Texture2D>();
            Texture2D texture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/brick");
            list.Add(texture);
            float centerFactor = (ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2) - ((texture.Width + bricksMargin) * 6) / 2;

            for (int n = 0; n < 6; n++) 
            {
                for (int i = 0; i < 6; i++)
                {
                    Bricks brick = new Bricks(list);
                    ListOfBricks.Add(brick);
                    brick.Position = new Vector2(((centerFactor + (brick.currentTexture.Width + bricksMargin) * n - 1)), 10 + ((brick.currentTexture.Height + bricksMargin) * i));
                }
             
            }
        }

        public void Down()
        {
            foreach (Bricks brick in listOfBricks)
            {
                brick.Position = new Vector2(brick.Position.X, brick.Position.Y + downSpeed);
            }
        }
    }
}
