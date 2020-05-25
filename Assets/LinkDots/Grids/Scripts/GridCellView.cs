using System;
using EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LinkDots
{
    public class GridCellView : MonoBehaviour
    {
        public GridDatum GridDatum { get; protected set; }

        public virtual void Initialize(GridDatum gridDatum)
        {
            GridDatum = gridDatum;
        }
      
    }
}