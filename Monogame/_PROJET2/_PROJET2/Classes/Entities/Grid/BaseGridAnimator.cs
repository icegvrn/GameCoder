using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Classe permettant d'effectuer des changements de position/déplacement sur les bricks présentent dans la grille
    /// </summary>
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


        // Méthode permettant de faire descendre toutes les briques de une case. Elle vérifie si ces briques sont disponibles ou pas
        public void Down()
        {
            for (int n = grid.GridElements.Count - 1; n >= 0; n--)
            {
                if (grid.GridElements[n] != null && grid.GridElements[n] is not Dice)
                {
                    int wantedPosition = (int)grid.GridElements[n].Position.Y - grid.BrickHeight / 2 + grid.BrickHeight * (int)grid.GridElements[n].Speed;

                    // On regarde si la position Y voulue est dans la zone de jeu sinon on choisit la case la plus proche
                    wantedPosition = ClampWantedPosition(wantedPosition);

                    // On récupère l'index de la case voulue
                    int wantedCaseIndex = grid.GetSlotIndexFromPosition(new Vector2(grid.GridElements[n].Position.X - grid.BrickWidth / 2, wantedPosition));

                    // On regarde si la case voulue est dispo du dessus, sinon on retourne la case dispo la plus proche
                    if (wantedCaseIndex >= 0)
                    {
                        int destIndex = GetDestinationIndex((Bricks)grid.GridElements[n], wantedCaseIndex);

                        // On bouge à la case désirée
                        MoveElements(n, destIndex);
                    }
                }
            }
        }

        // Méthode qui vérifie qu'une position Y voulue est bien dans l'air de jeu sinon on renvoi la position max ou min
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

        // Méthode permettant de déplacer un membre déjà présent dans la grille à un index précis
        public void MoveElements(int n, int destIndex)
        {
            Vector2 destination = grid.GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X + grid.BrickWidth / 2, destination.Y + grid.BrickHeight / 2);
            grid.GridElements[n].Move(destination);
            grid.GridElements[n].GridSlotNb = destIndex;
        }

        //Méthode permettant de déplacer un Ibrickable donné à un index précis
        public void MoveElements(IBrickable elem, int destIndex)
        {
            Vector2 destination = grid.GetPositionFromGrid(destIndex);
            destination = new Vector2(destination.X + grid.BrickWidth / 2, destination.Y + grid.BrickHeight / 2);
            elem.Move(destination);
            elem.GridSlotNb = destIndex;
        }

        // Méthode permettant de récupère l'index de la case disponible la plus proche de wantedCaseIndex. Si pas disponible, on essaie la case du dessus jusqu'à trouver
        private int GetDestinationIndex(Bricks brick, int wantedCaseIndex)
        {
            while (IsOccupied(brick, wantedCaseIndex))
            {
                wantedCaseIndex = grid.GetSlotIndexAbovePosition(wantedCaseIndex);
            }

            return wantedCaseIndex;
        }


        // Méthode permettant de dire si la case est occupée par une autre brique que celle qui souhaite s'y rendre
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
