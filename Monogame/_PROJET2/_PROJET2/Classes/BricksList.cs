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

        public void CreateBricksWall()
        {
            ListOfBricks = new List<Bricks>();

            for (int n = 0; n < 11; n++) 
            {
                for (int i = 0; i < 3; i++)
                {
                    List<Texture2D> list = new List<Texture2D>();
                    list.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/brick"));
                    Bricks brick = new Bricks(list);
                    ListOfBricks.Add(brick);
                    brick.Position = new Vector2(brick.currentTexture.Width * n - 1, 10 + brick.currentTexture.Height * i);
                }
             
            }
        }
       
    }
}
