using BricksGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            private float bricksMargin = 0f;
            private float downSpeed = 56f;
        private Vector2 Position;

        public BaseGrid(int colNb, int linNb)
            {
            Position = new Vector2(brickWidth, brickHeight*2);
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            columnsNb = colNb;
            linesNb = linNb;


            for (int n=0; n<linesNb; n++)
            {
                for (int i=0; i<columnsNb; i++)
                {
                    Debug.WriteLine(" POsition X " + Position.X + "Brick width " + brickWidth + " i " + (i));
                    Debug.WriteLine(Position.X + ((brickWidth * 1.5f) * (i)));
                    Vector2 vector = new Vector2(Position.X + brickWidth/2 +((brickWidth) * (i)), Position.Y + brickHeight/2+ (brickHeight * n));
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

        public void Clear()
        {
     
    
        }



    }

   

}

