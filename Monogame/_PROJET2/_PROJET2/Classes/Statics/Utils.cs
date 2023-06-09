﻿using Microsoft.Xna.Framework;
using System;

namespace BricksGame
{

    public static class Utils
    {
        static Random RandomGen = new Random();

        // Permet de générer un chiffre random
        public static int GetRandomInt(int p_Min, int p_Max)
        {
            return RandomGen.Next(p_Min, p_Max + 1);
        }


        // Permet de "fixer" l'aléatoire, pratique pour les beugs
        public static void SetRandomSeed(int p_Seed)
        {
            RandomGen = new Random(p_Seed);
        }

        public static bool CollideByBox(GameObject p_1, GameObject p_2)
        {
            return p_1.BoundingBox.Intersects(p_2.BoundingBox);
        }

        public static double Sqr(double db)
        {
            return (db * db);
        }

        // Formule de calcul de distance euclidienne
        public static double calcDistance(float xA, float yA, float xB, float yB)
        {
            return Math.Sqrt(Sqr(xB - xA) + Sqr(yB - yA));
        }


        // Formule de calcul d'un angle radian entre un point A et un point B
        public static double calcAngle(float x1, float y1, float x2, float y2)
        {
            return Math.Atan2(y2 - y1, x2 - x1);
        }

        //Même formule mais qui prend directement la souris comme second point de référence
        public static double calcAngleWithMouse(float x1, float y1)
        {
           Vector2 mouse = ServiceLocator.GetService<IInputService>().GetMousePosition();
            float x2 = mouse.X;
            float y2 = mouse.Y;
            return Math.Atan2(y2 - y1, x2 - x1);
        }

    }
}
