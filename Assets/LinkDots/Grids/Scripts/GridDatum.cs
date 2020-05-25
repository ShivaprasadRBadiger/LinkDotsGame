using System;
using UnityEngine;

namespace LinkDots
{
    [Serializable]
    public class GridDatum
    {
        public int PosX;
        public int PosY;
        public ColorData ColorData;
        public GridDatum(int i, int j)
        {
            PosX = i;
            PosY = j;
            ColorData = null;
        }
    }
}