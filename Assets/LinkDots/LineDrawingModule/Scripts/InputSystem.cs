using EventSystem;
using UnityEngine;

namespace LinkDots
{
    public class InputSystem:MonoBehaviour
    {
        private Camera _mainCamera;
        private GridCellView _currentCell = null;
        void Awake()
        {
            _mainCamera = Camera.main;
        }
        void Update()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = _mainCamera.nearClipPlane;
            Ray ray2D = _mainCamera.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray2D.origin,ray2D.direction,Color.red);
            RaycastHit2D hitData = Physics2D.Raycast(ray2D.origin, ray2D.direction);
            if (hitData.collider!=null)
            {
                var gridCell=hitData.collider.gameObject.GetComponent<GridCellView>();
                if (Input.GetMouseButtonDown(0))
                {
                    if (gridCell is DotView)
                    {
                        EventManager.Raise(CustomEventType.OnBeginDragOnDot,gridCell as DotView);
                        return;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (gridCell is DotView)
                    {
                        EventManager.Raise(CustomEventType.OnEndedDragOnDot,gridCell as DotView);
                        return;
                    }
                    else
                    {
                        EventManager.Raise(CustomEventType.OnGridCellEndedDrag,gridCell);
                        return;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (_currentCell==null || gridCell!=_currentCell)
                    {
                        EventManager.Raise(CustomEventType.OnGridCellEnter,gridCell);
                        _currentCell = gridCell;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    EventManager.Raise(CustomEventType.OnCancelDrawLine);
                }
            }
        }
    }
}