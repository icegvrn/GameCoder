using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _TEMPLATE.Classes
{
    public static class Utils
    {
        static Random RandomGen = new Random();
        public static int GetRandomInt(int p_Min, int p_Max)
        {
            return RandomGen.Next(p_Min, p_Max + 1);
        }

        /// <summary>
        /// Permet de "fixer" l'aléatoire, pratique pour les beugs
        /// </summary>
        /// <param name="p_Seed"></param>
        public static void SetRandomSeed(int p_Seed)
        {
            RandomGen = new Random(p_Seed);
        }

        public static bool CollideByBox(IActor p_1, IActor p_2)
        {
            return p_1.BoundingBox.Intersects(p_2.BoundingBox);
        }

    }
}
