using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public static class PlayerState
    {
        private static int points;
        private static int lifes;
        public static int Points { get { return points; } private set { points = value; } }
        public static int Lifes { get { return lifes; } private set { if (points - value < 0) { points = 0;} else { lifes = value;} } }

        static PlayerState()
        {
            Points = 0;
            Lifes = 5;
        }

        public static void AddPoints(int p_points)
        {
            Points += p_points;
        }

        public static void SubsPoints(int p_points)
        {
            Points -= p_points;
        }

        public static void MultiplyPoints(int p_multiplicator)
        {
            Points *= p_multiplicator;
        }

        public static void MultiplyPoints(int p_points, int p_multiplicator)
        {
            Points += p_points * p_multiplicator;
        }

        public static void DividePoints(int p_divider)
        {
            Points /= p_divider;
        }

    }
}
