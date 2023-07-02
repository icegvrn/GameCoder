
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace BricksGame
{
    public class BaseGrid
    {
        private List<Vector2> slotPositions;
        private List<Rectangle> slotRectangles;
        public List<Rectangle> SlotRectangles { get { return slotRectangles; } }
        private List<IBrickable> gridElements;
        public List<IBrickable> GridElements { get { return gridElements; } }
        private int columnsNb;
        private int linesNb;
        private const int brickWidth = 55;
        private const int brickHeight = 55;
        public int BrickWidth { get { return brickWidth; } }
        public int BrickHeight { get { return brickHeight; } }

        public int WidthInColumns { get { return columnsNb; } }
        public int HeightInLines { get { return linesNb; } }

        private const int downSpeed = 55;
        private Vector2 Position;
        public int minDestination;
       public int maxDestination;

        private Texture2D gridTexture;
        private Vector2 gridTexturePos;

        public BaseGrid(int colNb, int linNb)
        {
            InitGrid(colNb, linNb);
            CreateSlotsFromGrid(columnsNb, linesNb);

        }

        private void InitGrid(int colNb, int linNb)
        {
            gridTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/grid");
            gridTexturePos = new Vector2(0, 9);
            Position = new Vector2(30, 90);
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            slotRectangles = new List<Rectangle>();
            columnsNb = colNb;
            linesNb = linNb;
    }

        private void CreateSlotsFromGrid(int columnsNb, int linesNb)
        {
            minDestination = ServiceLocator.GetService<PlayerArea>().area.Top;
            maxDestination = (int)Position.Y + (brickHeight / 2 + brickHeight * (linesNb - 4));
            for (int n = 0; n < linesNb; n++)
            {
                for (int i = 0; i < columnsNb; i++)
                {
                    Vector2 vector = new Vector2(Position.X + brickWidth / 2 + ((brickWidth) * (i)), Position.Y + brickHeight / 2 + (brickHeight * n));
                    slotPositions.Add(vector);
                    Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, brickWidth, brickHeight);
                    slotRectangles.Add(rectangle);

                }
            }
        }

        public void AddBrickable(IBrickable elem, int index)
        {
            gridElements.Add(elem);
            elem.GridSlotNb = index;
            elem.Position = GetPositionFromGrid(index);
            MoveElements(elem, index);
        }


        public Vector2 GetPositionFromGrid(int index)
        {
            return slotPositions[index];
        }

        public int GetSlotIndexFromPosition(Vector2 position)
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
                    int wantedPosition = (int)gridElements[n].Position.Y-brickHeight/2 + downSpeed * (int)gridElements[n].Speed;

                    wantedPosition = ClampWantedPosition(wantedPosition);

                    int wantedCaseIndex = GetSlotIndexFromPosition(new Vector2(gridElements[n].Position.X - brickWidth / 2, wantedPosition));

                    if (wantedCaseIndex >= 0)
                    {
                        int destIndex = GetDestinationIndex((Bricks)gridElements[n], wantedCaseIndex);

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

        private void MoveElements(int n, int destIndex)
        {
            Vector2 destination = GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X+brickWidth/2, destination.Y+brickHeight/2);
            gridElements[n].Move(destination);
            gridElements[n].GridSlotNb = destIndex;
        }

        private void MoveElements(IBrickable elem, int destIndex)
        {
            Vector2 destination = GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X + brickWidth / 2, destination.Y + brickHeight / 2);
            elem.Move(destination);
            elem.GridSlotNb = destIndex;
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

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gridTexture, gridTexturePos, Color.White);
        }

        public void DrawSlots(SpriteBatch spriteBatch, bool showOutOfLimitSlot)
        {
            foreach (Rectangle rect in slotRectangles)
            {
                if (!showOutOfLimitSlot)
                {
                    if (rect.Top <= maxDestination)
                    {
                        spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.White);
                        spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.White);
                        spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.White);
                        spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.White);
                    }
                }
                else
                {
                    {
                spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.White);
                spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.White);
                spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.White);
                spriteBatch.Draw(AssetsManager.blankTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.White);
                    }
                     
                }
              
            }
         
        }

    }

}
