using BricksGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BricksGame
{
    public class BaseGrid
    {
            private List<Vector2> slotPositions;
            private List<IBrickable> gridElements;
            public List<IBrickable> GridElements { get { return gridElements; } }
            private int columnsNb;
            private int linesNb;
            private float brickWidth = 56f;
            private float brickHeight = 56f;
            private float bricksMargin = 30f;
            private float downSpeed = 15f;

        public BaseGrid(int colNb, int linNb)
            {
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            columnsNb = colNb;
            linesNb = linNb;
            float centerFactor = (ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2) - ((brickWidth + bricksMargin) * 6) / 2;


            for (int n=0; n<linesNb; n++)
            {
                for (int i=0; i<columnsNb; i++)
                {
                    Vector2 vector = new Vector2((((centerFactor+(brickWidth + bricksMargin) * i - 1))) + brickWidth*0.5f, 10 + ((brickHeight + bricksMargin) * n)  +brickHeight * 0.5f);
                    slotPositions.Add(vector);
                }
            }
            }

        public void AddBrickable(IBrickable elem, int index)
        {
            gridElements.Add(elem);  
            elem.Position = GetPositionFromGrid(index);
        }


            private Vector2 GetPositionFromGrid(int index)
            {
                return slotPositions[index];
            }
        public void Down()
        {
            foreach (IBrickable brick in gridElements)
            {
                if (brick != null)
                {
                    brick.Position = new Vector2(brick.Position.X, brick.Position.Y + downSpeed);
                }
               
            }
        }

    }

   

}

