﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class LevelList
    {
        public int LevelsNb { get; set; }
        public Level[] Levels { get; set; }

        public LevelList()
        {
            LevelsNb = 1;
            Levels = new Level[1];
            Levels[0] = new Level();
        }
    }
}
