
namespace BricksGame
{
    /// <summary>
    /// Classe contenant le pouvoir permettant de détruire tous les monstres d'un même type
    /// </summary>
    public class SameTypePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public override string description { get; set; }
        public SameTypePower() : base()
        {
            description = "DESTROY ALL SAME MONSTERS IN ONCE!";
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
                            if (element.Fighter.Strenght == m_brick.Fighter.Strenght)
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
