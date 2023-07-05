using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class BaseGridAnimator
    {
        private BaseGrid grid;
        private int minDestination;
        private int maxDestination;
        public BaseGridAnimator(BaseGrid p_grid, int p_minPosition, int p_maxPositon) 
        {
            grid = p_grid;
            minDestination = p_minPosition;
            maxDestination = p_maxPositon;
        }


        public void Down()
        {
            for (int n = grid.GridElements.Count - 1; n >= 0; n--)
            {
                if (grid.GridElements[n] != null && grid.GridElements[n] is not Dice)
                {
                    int wantedPosition = (int)grid.GridElements[n].Position.Y - grid.BrickHeight / 2 + grid.BrickHeight * (int)grid.GridElements[n].Speed;

                    wantedPosition = ClampWantedPosition(wantedPosition);

                    int wantedCaseIndex = grid.GetSlotIndexFromPosition(new Vector2(grid.GridElements[n].Position.X - grid.BrickWidth / 2, wantedPosition));

                    if (wantedCaseIndex >= 0)
                    {
                        int destIndex = GetDestinationIndex((Bricks)grid.GridElements[n], wantedCaseIndex);

                        // Move to the desired case
                        MoveElements(n, destIndex);

                    }
                }
            }
        }


        private int ClampWantedPosition(int wantedPosition)
        {
            if (wantedPosition >= maxDestination)
            {
                wantedPosition = maxDestination;
            }
            else if (wantedPosition <= minDestination)
            {
                wantedPosition = minDestination;
            }
            return wantedPosition;
        }

        public void MoveElements(int n, int destIndex)
        {
            Vector2 destination = grid.GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X + grid.BrickWidth / 2, destination.Y + grid.BrickHeight / 2);
            grid.GridElements[n].Move(destination);
            grid.GridElements[n].GridSlotNb = destIndex;
        }

        public void MoveElements(IBrickable elem, int destIndex)
        {
            Vector2 destination = grid.GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X + grid.BrickWidth / 2, destination.Y + grid.BrickHeight / 2);
            elem.Move(destination);
            elem.GridSlotNb = destIndex;
        }



        private int GetDestinationIndex(Bricks brick, int wantedCaseIndex)
        {
            while (IsOccupied(brick, wantedCaseIndex))
            {
                wantedCaseIndex = grid.GetSlotIndexAbovePosition(wantedCaseIndex);
            }

            return wantedCaseIndex;
        }


        private bool IsOccupied(Bricks brick, int wantedCaseIndex)
        {
            foreach (Bricks nBrick in grid.GridElements)
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
    }
}
