

namespace BricksGame
{
    /// <summary>
    /// Classe contenant le pouvoir permettant de détruire tous les monstres sur une même ligne
    /// </summary>
    public class LinePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public override string description { get; set; }
        public LinePower() : base()
        {
             description = "DESTROY A LINE OF MONSTERS!";
        }

        public override void Activate()
        {
            powerAvailable = false;
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
            if (!powerUsed)
            {
                int lineIndex = (int)(brick.GridSlotNb / grid.WidthInColumns);
                int startIndex = lineIndex * grid.WidthInColumns;
                int endIndex = startIndex + grid.WidthInColumns - 1;

                for (int i = grid.GridElements.Count - 1; i >= 0; i--)
                {
                    if (grid.GridElements[i] != null && grid.GridElements[i] is Monster)
                    {
                        if (grid.GridElements[i].GridSlotNb >= startIndex && grid.GridElements[i].GridSlotNb <= endIndex)
                        {
                            if (grid.GridElements[i] is Monster)
                            {
                                Monster element = (Monster)grid.GridElements[i];
                                element.Kill();
                            }
                        }
                    }
                }
                powerUsed = true;
                powerCharged = false;
       
            }

        }
    }
}
