using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public static class PlayerState
    {
        private static int points;
        private static int life;
        public static int Points { get { return points; } private set { points = value; } }
        public static int Life { get { return life; } private set { if (life < 0) { life = 0;} else { life = value;} } }

        static PlayerState()
        {
            Points = 0;
        }

        public static void SetLife(int p_life)
        {
            Life = p_life;
        }

        public static void AddLife(int p_life)
        {
            Life += p_life;
        }


        public static void SubsLife(int p_damage)
        {
            Life -= p_damage;
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
