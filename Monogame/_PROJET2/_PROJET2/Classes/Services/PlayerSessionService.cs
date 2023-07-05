

namespace BricksGame
{
    public class PlayerSessionService : ISessionService
    {
        private int points;
        private int maxPoints;
        private int life;

        public PlayerSessionService()
        {
            points = 0;
        }

        public int GetPoints()
        {
            return points;
        }

        public int GetMaxPoints()
        {
            return maxPoints;
        }

        public int GetLife()
        {
            return life;
        }
        public void SetLife(int p_life)
        {
            life = p_life;
        }

        public void SetPoints(int p_points)
        {
            points = p_points;
        }

        public void SetMaxPoints(int p_points)
        {
            maxPoints = p_points;
        }

        public void AddLife(int p_life)
        {
            life += p_life;
        }


        public void SubsLife(int p_damage)
        {
            life -= p_damage;
        }

        public void AddPoints(int p_points)
        {
            points += p_points;
        }


        public void SubsPoints(int p_points)
        {
            points -= p_points;
        }

        public void MultiplyPoints(int p_multiplicator)
        {
            points *= p_multiplicator;
        }

        public void MultiplyPoints(int p_points, int p_multiplicator)
        {
            points += p_points * p_multiplicator;
        }

        public void DividePoints(int p_divider)
        {
            points /= p_divider;
        }

    }
}
