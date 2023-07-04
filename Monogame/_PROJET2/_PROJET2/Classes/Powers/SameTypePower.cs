
namespace BricksGame
{
    public class SameTypePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public SameTypePower() : base()
        {
        }

        public override void Activate()
        {
            powerAvailable = false;
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
            if (!powerUsed)
            {
                if (brick is Monster)
                {
                    Monster m_brick = (Monster)brick;

                    for (int i = grid.GridElements.Count - 1; i >= 0; i--)
                    {
                        if (grid.GridElements[i] != null && grid.GridElements[i] is Monster)
                        {
                            Monster element = (Monster)grid.GridElements[i];
                            if (element.Power == m_brick.Power)
                            {
                                element.Kill();
                            }
                        }

                    }
                    m_brick.Kill();
                }
                powerUsed = true;
                powerCharged = false;
          
            }
        }
    }
}
