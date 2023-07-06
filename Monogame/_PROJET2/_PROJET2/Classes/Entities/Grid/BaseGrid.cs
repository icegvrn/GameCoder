
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    /// Grille permettant d'y insérer des briques (IBrickable). Principalement utilisée pour mettre des dés et des monstres. Elle contient principalement les données chiffrées de la grille : nombre de lignes, colonnes, index des cases etc
    /// </summary>
    public class BaseGrid
    {

        // Définition de la grille et des cases
        private Vector2 Position;
        private int columnsNb;
        private int linesNb;
        private const int brickWidth = 55;
        private const int brickHeight = 55;
        // Limites de la grille
        public int minDestination;
        public int maxDestination;

        // Accesseurs publics
        public int BrickWidth { get { return brickWidth; } }
        public int BrickHeight { get { return brickHeight; } }
        public int WidthInColumns { get { return columnsNb; } }
        public int HeightInLines { get { return linesNb; } }

        // Elements présents dans la grille
        private List<Vector2> slotPositions;
        private List<IBrickable> gridElements;
        public List<IBrickable> GridElements { get { return gridElements; } }


        // Composants
        private BaseGridRenderer gridRenderer;
        private BaseGridAnimator gridAnimator;

        public BaseGrid(int colNb, int linNb)
        {
            InitGrid(colNb, linNb);
            CreateSlotsFromGrid(columnsNb, linesNb);
            InitGridAnimator();
        }

        // Initialisation de la grille
        private void InitGrid(int colNb, int linNb)
        {
            Position = new Vector2(30, 90);
            slotPositions = new List<Vector2>();
            gridElements = new List<IBrickable>();
            columnsNb = colNb;
            linesNb = linNb;
        }

        // Initialisation du gridAnimator, utilisé pour gérer les mouvements des Ibrickable sur la grille
        public void InitGridAnimator()
        {
            minDestination = ServiceLocator.GetService<PlayerArea>().Area.Top;
            maxDestination = (int)Position.Y + (brickHeight / 2 + brickHeight * (linesNb - 4));
            gridAnimator = new BaseGridAnimator(this, minDestination, maxDestination);
        }

        // Création de la grille en créant des slots en list vector2
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
            gridRenderer = new BaseGridRenderer(slotPositions, brickWidth, brickHeight);
        }

        // Ajout d'élément à un index correspondant à un slot de la grille
        public void AddBrickable(IBrickable elem, int index)
        {
            gridElements.Add(elem);
            elem.GridSlotNb = index;
            elem.Position = GetPositionFromGrid(index);
            MoveElements(elem, index);
        }

        // Renvoi une position en Vector2 quand on lui passe un numéro de slot
        public Vector2 GetPositionFromGrid(int index)
        {
            return slotPositions[index];
        }

        // Renvoi un numéro de slot quand on lui envoi une position
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


        // Renvoi le slot juste au-dessus (utilisé notamment par l'animator s'il cherche une case à occuper)
        public int GetSlotIndexAbovePosition(int index)
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

        // Appelle la fonction de l'animator permettant de faire descendre toute la grille de une case.
        public void Down()
        {
            gridAnimator.Down();
        }


        // Appelle l'animator pour bouger un élément à un slot précis.
        private void MoveElements(IBrickable elem, int destIndex)
        {
            gridAnimator.MoveElements(elem, destIndex);
        }



        // Nettoie tous les éléments de la grille
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
           gridRenderer.DrawGrid(spriteBatch);  
        }

        // Debug : Dessine tous les slots de la grille 
        public void DrawSlots(SpriteBatch spriteBatch, bool showOutOfLimitSlot)
        {
            gridRenderer.DrawSlots(spriteBatch, showOutOfLimitSlot, maxDestination);
        }

        // Retourne la grille sous forme de rectangle, utilisé notamment si besoin de calculer la présence de la souris sur une case
        public List<Rectangle> GetSlotsByRectangles()
        {
            return gridRenderer.SlotRectangles;
        }

    }

}
