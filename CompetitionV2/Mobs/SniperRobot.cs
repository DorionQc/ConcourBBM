﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class SniperRobot : Enemy
    {
        public SniperRobot(int x, int y, Map m) : base(x, y, m, 0.18f) // Speedfactor changed there
        {
            _Hp = 26;
            // _Weapon = SniperSemiAuto;
        }
    }
}
