using UnityEngine;

namespace LinkDots
{
    public static class GridDatumExtension
    {
        public static Vector3  GetLocalPositionFromIndices(this GridDatum gridDatum)
        {
            return new Vector3(gridDatum.PosX , gridDatum.PosY ,0f);
        }
    }
}