
using System.Collections.Generic;

using System.Numerics;


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

        public void Clear()
        {

        }
        public void Down()
        {
            foreach (IBrickable brick in gridElements)
            {
                if (brick != null && brick is not Dice)
                {
                    Vector2 destination = new Vector2(brick.Position.X, brick.Position.Y + downSpeed * brick.Speed);

                    if (destination.Y >= brickHeight * (linesNb - 1))
                    {
                        destination = new Vector2(brick.Position.X, brickHeight * (linesNb - 1));
                    }



                    bool pathFind = false;
                    while (!pathFind)
                    {
                        bool occuped = false;

                        foreach (IBrickable brick2 in gridElements)
                        {
                            if (brick2 != null && brick2 != brick && brick2.Position == destination && brick2 is not Dice)
                            {
                                
                                occuped = true;
                                break;
                            }
                        }

                        if (!occuped)
                        {
                            pathFind = true;
                            break;
                        }
                        else
                        {
                            destination = new Vector2(destination.X, destination.Y - brickHeight);
                        }
                    }

                    brick.Position = destination;
                }
            }
        }

    }

   

}

