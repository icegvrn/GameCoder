using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class ExplodePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public ExplodePower() : base()
        {
        }

        public override void Activate()
        {
            powerAvailable = false;
            // Debug.WriteLine("POUVOIR ACTIVE");
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
            if (!powerUsed)
            {
                Debug.WriteLine("EXPLODE : POUVOIR UTILISE");

                int lineIndex = (int)(brick.GridSlotNb / grid.WidthInColumns);
                int colIndex = (int)(brick.GridSlotNb % grid.WidthInColumns);

                int startIndex = Math.Max(lineIndex - 1, 0) * grid.WidthInColumns + Math.Max(colIndex - 1, 0);
                int endIndex = Math.Min(lineIndex + 1, grid.HeightInLines - 1) * grid.WidthInColumns + Math.Min(colIndex + 1, grid.WidthInColumns - 1);

                Debug.WriteLine("Je cherche à éliminer de l'index " + startIndex + " jusqu'à l'index " + endIndex);

                for (int i = grid.GridElements.Count - 1; i >= 0; i--)
                {
                    if (grid.GridElements[i] != null && grid.GridElements[i] is Monster)
                    {
                        int elementSlotIndex = (int)grid.GridElements[i].GridSlotNb;

                        int elementLineIndex = elementSlotIndex / grid.WidthInColumns;
                        int elementColIndex = elementSlotIndex % grid.WidthInColumns;

                        if (elementLineIndex >= lineIndex - 1 && elementLineIndex <= lineIndex + 1 &&
                            elementColIndex >= colIndex - 1 && elementColIndex <= colIndex + 1)
                        {
                            Monster element = (Monster)grid.GridElements[i];
                            element.Kill();
                        }
                    }
                }

                powerUsed = true;
                powerCharged = false;

            }
        }
    }
}
