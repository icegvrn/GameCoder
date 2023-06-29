
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;


namespace BricksGame
{
    public class BaseGrid
    {
        private List<Vector2> slotPositions;
        private List<IBrickable> gridElements;
        public List<IBrickable> GridElements { get { return gridElements; } }
        private int columnsNb;
        private int linesNb;
        private const int brickWidth = 56;
        private const int brickHeight = 56;
        private const int downSpeed = 56;
        private Vector2 Position;

        public BaseGrid(int colNb, int linNb)
        {
            InitGrid(colNb, linNb);
            CreateSlotsFromGrid(columnsNb, linesNb);
        }

        private void InitGrid(int colNb, int linNb)
        {
            Position = new Vector2(brickWidth, brickHeight * 2);
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            columnsNb = colNb;
            linesNb = linNb;
        }

        private void CreateSlotsFromGrid(int columnsNb, int linesNb)
        {
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
            elem.GridSlotNb = index;
            elem.Position = GetPositionFromGrid(index);
        }


        private Vector2 GetPositionFromGrid(int index)
        {
            return slotPositions[index];
        }

        private int GetSlotIndexFromPosition(Vector2 position)
        {
            for (int i = 0; i < slotPositions.Count; i++)
            {
                if (position == slotPositions[i])
                {
                    return i;
                }
            }
            return -1;
        }

 

        public void Down()
        {
            for (int n = gridElements.Count - 1; n >= 0; n--)
            { 
                if (gridElements[n] != null && gridElements[n] is not Dice)
                {
                    int wantedPosition = (int)gridElements[n].Position.Y + downSpeed * (int)gridElements[n].Speed;
                    
                    wantedPosition = ClampWantedPosition(wantedPosition);
   
                    int wantedCaseIndex = GetSlotIndexFromPosition(new Vector2(gridElements[n].Position.X, wantedPosition));
 
                    if (wantedCaseIndex >= 0)
                    {
                        int destIndex = GetDestinationIndex((Bricks)gridElements[n], wantedCaseIndex);
                        MoveElements(n, destIndex);
                    }
                }
            }
        }


        private int ClampWantedPosition(int wantedPosition)
        {
            int minDestination = ServiceLocator.GetService<PlayerArea>().area.Top;
            int maxDestination = brickHeight / 2 + (brickHeight * (linesNb - 2));

            if (wantedPosition > maxDestination)
            {
                wantedPosition = maxDestination;
            }
            else if (wantedPosition < minDestination)
            {
                wantedPosition = minDestination;
            }
            return wantedPosition;
        }

       private void MoveElements(int n, int destIndex)
        {
            gridElements[n].Move(GetPositionFromGrid(destIndex));
            gridElements[n].GridSlotNb = destIndex;
        }


        private int GetDestinationIndex(Bricks brick, int wantedCaseIndex)
        {
            while (IsOccupied(brick, wantedCaseIndex))
            {
                wantedCaseIndex = GetSlotIndexAbovePosition(wantedCaseIndex);
            }

            return wantedCaseIndex;
        }


        private bool IsOccupied(Bricks brick, int wantedCaseIndex)
        {
            foreach (Bricks nBrick in gridElements)
            {
                if (nBrick != brick)
                {
                    if (nBrick != null && nBrick.GridSlotNb == wantedCaseIndex && !nBrick.IsDestroy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int GetSlotIndexAbovePosition(int index)
        {
            int line = index / columnsNb;
            int col = index - line * columnsNb;

            if (line == 0)
            {
                return -1; 
            } 
            
            else
            {
                index = (line - 1) * columnsNb + col;
                return index;
            }
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
    }

    }

