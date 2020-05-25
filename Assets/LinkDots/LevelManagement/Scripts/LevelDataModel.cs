using System;
using System.Collections.Generic;

namespace LinkDots
{
    [Serializable]
    public class LevelDataModel
    {
        public int Id;
        public int GridSize;
        public List<GridDatum> GridData;
    }
}
