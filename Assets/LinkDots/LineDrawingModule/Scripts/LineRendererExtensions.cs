using System.Collections.Generic;
using UnityEngine;

namespace LinkDots
{
    public static class LineRendererExtensions
    {
        public static void SetPositions(this LineRenderer lineRenderer,List<Vector3Int> positions)
        {
            lineRenderer.positionCount = positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i,positions[i]);
            }
        }
    }
}