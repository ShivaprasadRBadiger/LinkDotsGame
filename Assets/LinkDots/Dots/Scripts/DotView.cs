using EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LinkDots
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DotView : GridCellView
    {
        private SpriteRenderer _spriteRenderer;
        
        public override void Initialize(GridDatum gridDatum)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            GridDatum = gridDatum;
            if (ColorUtility.TryParseHtmlString(GridDatum.ColorData.ColorHexString, out var colorObj))
            {
                _spriteRenderer.color =colorObj;
            }
            else
            {
                Debug.LogError($"Invalid color code passed! - {GridDatum.ColorData.ColorHexString}");
            }
        }
    }
}
