
using System;
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
        private float downSpeed = 56f;
        private Vector2 Position;

        public BaseGrid(int colNb, int linNb)
        {
            Position = new Vector2(brickWidth, brickHeight * 2);
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            columnsNb = colNb;
            linesNb = linNb;


            for (int n = 0; n < linesNb; n++)
            {
                for (int i = 0; i < columnsNb; i++)
                {
                    Vector2 vector = new Vector2(Position.X + brickWidth / 2 + ((brickWidth) * (i)), Position.Y + brickHeight / 2 + (brickHeight * n));
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
            for (int n = gridElements.Count - 1; n >= 0; n--)
            {
                if (gridElements[n] is IDestroyable)
                {
                    IDestroyable element = (IDestroyable)gridElements[n];
                    element.Destroy();
                }
            }
        }

        public void Down()
        {
            for (int n = gridElements.Count - 1; n >= 0; n--)
            {
                if (gridElements[n] != null && gridElements[n] is not Dice)
                {
                 


                    float wantedCase = gridElements[n].Position.Y + downSpeed * gridElements[n].Speed;
                    Vector2 destination = new Vector2(gridElements[n].Position.X, gridElements[n].Position.Y);

                    if (wantedCase > brickHeight * (linesNb - 1))
                    {
                        wantedCase = brickHeight * (linesNb - 1);
                    }

                    while (gridElements[n].Position.Y < wantedCase)
                    {
                        bool occupied = false;

                        foreach (Bricks brick2 in gridElements)
                        {
                            if (brick2 != null && brick2 != gridElements[n] && brick2.Position == new Vector2(gridElements[n].Position.X, wantedCase) && !brick2.IsDestroy)
                            {
                                occupied = true;
                                break;
                            }
                        }

                        if (!occupied)
                        {
                            destination = new Vector2(gridElements[n].Position.X, wantedCase);
                            break;
                        }
                        else
                        {
                            wantedCase -= brickHeight;   
                            
                            //Si toutes les position au dessus sont occupées
                            if (wantedCase < 0)
                            {
                                break;
                            }
                        }
                    }

                    gridElements[n].Move(destination);
                }
            }
        }
    }

    }

