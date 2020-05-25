using System.Collections.Generic;
using System.ComponentModel;
using EventSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LinkDots
{
   public class LineDrawBehaviour : MonoBehaviour 
   {
      [SerializeField] private LineRenderer _linePrefab;

      private Dictionary<int, LineRenderer> _lineLookup = new Dictionary<int, LineRenderer>();
      private Dictionary<int, List<Vector3Int>> _completedLines = new Dictionary<int, List<Vector3Int>>();
      private Dictionary<int, List<GridDatum>> _completedLineGridDatum = new Dictionary<int, List<GridDatum>>();

      private GridDatum _currentGridDatum;

      
      private LineDrawState _lineDrawState;
      private ColorData _currentLineColorData;
      private LineRenderer _currentLineRenderer;
      private List<Vector3Int> _currentLineData;
      private List<GridDatum> _currentLineGridData ;
      private DotView _currentDotOrigin;
   
      void OnEnable()
      {
         Debug.Log("Registering events");
         EventManager.RegisterHandler(CustomEventType.OnBeginDragOnDot, OnDragBeganOnDotHandler);
         EventManager.RegisterHandler(CustomEventType.OnGridCellEnter, OnGridCellEnterHandler);
         EventManager.RegisterHandler(CustomEventType.OnGridCellEndedDrag, OnGridCellEndedDragHandler);
         EventManager.RegisterHandler(CustomEventType.OnCancelDrawLine, OnCancelDrawLineHandler);
      }

      private void OnCancelDrawLineHandler(object args)
      {
         CancelCurrentLine();
      }


      private void OnGridCellEndedDragHandler(object obj)
      {
         if (_lineDrawState != LineDrawState.Drawing) return;

           var gridCellView= (GridCellView) obj;
           var gridDatum = gridCellView.GridDatum;
           if (gridDatum == null)
           {
              throw new InvalidEnumArgumentException();
           }
           Debug.Log("Grid OnGridCellEndedDragHandler");
           CancelCurrentLine();
      }

      private void CancelCurrentLine()
      {
         if (_currentLineRenderer == null)
            return;
         
         _currentLineRenderer.positionCount = 0;
         _currentLineRenderer = null;
         _currentLineColorData = null;
         _currentDotOrigin = null;
         foreach (var gridDatum in _currentLineGridData)
         {
            gridDatum.ColorData = null;
         }
         _lineDrawState = LineDrawState.None;
      }

      private void OnGridCellEnterHandler(object grid)
      {
         if (_lineDrawState != LineDrawState.Drawing) return;

         var cellView= (GridCellView)grid;
         if (cellView == null)
         {
            throw new InvalidEnumArgumentException();
         }
         var gridDatum = cellView.GridDatum;
         var isDot = (cellView is DotView);
         Debug.Log($"IS DOT :{isDot}");
         if (!isDot)
         {
            if (gridDatum.ColorData != null )
            {
               Debug.Log($"gridDatum.ColorData!=null=> {gridDatum.ColorData.ColorId!= _currentLineColorData.ColorId}");

               if (gridDatum.ColorData.ColorId != _currentLineColorData.ColorId && AreAdjacent(gridDatum, _currentGridDatum)) // Clear only if im going to continue along
               {
                  Debug.Log($"Clearing line {gridDatum.ColorData.ColorId}");
                  ClearLine(gridDatum.ColorData.ColorId);
               }
            }
            else
            {
               Debug.Log("Color Data is null");
            }
            if (IsOverlappingCurrentLine(gridDatum))
            {
               return;
            }
         }
         else if (cellView.GridDatum.ColorData.ColorId != _currentLineColorData.ColorId)
         {
            return;
         }
         else if (cellView == _currentDotOrigin)
         {
            return;
         }
         
         if (_currentGridDatum!=null && !AreAdjacent(gridDatum, _currentGridDatum))
         {
            return;
         }
         _currentGridDatum = gridDatum;
          ExtendCurrentLine(gridDatum,isDot);
          
          if (!(cellView is DotView) || cellView == _currentDotOrigin ||
              cellView.GridDatum.ColorData?.ColorId != _currentLineColorData.ColorId) return;
          
          EventManager.Raise(CustomEventType.OnLineCompleted,_currentLineColorData.ColorId);
          _completedLines.Add(_currentLineColorData.ColorId, _currentLineData);
          _completedLineGridDatum.Add(_currentLineColorData.ColorId,_currentLineGridData);
          FinishDrawing();
      }

      private void FinishDrawing()
      {
         _currentDotOrigin = null;
         _currentGridDatum = null;
         _currentLineData = null;
         _currentLineRenderer = null;
         _currentLineData = null;
         _lineDrawState = LineDrawState.None;
      }

      private void ClearLine(int colorDataColorId)
      {
         if (!_lineLookup.ContainsKey(colorDataColorId))
         {
            return;
         }
         _lineLookup[colorDataColorId].positionCount = 0;
         if (!_completedLines.ContainsKey(colorDataColorId))
         {
            return;
         }
         _completedLines[colorDataColorId].Clear();
         _completedLines.Remove(colorDataColorId);
         if (!_completedLineGridDatum.ContainsKey(colorDataColorId))
         {
            return;
         }
         foreach (var gridDatum in _completedLineGridDatum[colorDataColorId])
         {
            gridDatum.ColorData = null;
         }
         _completedLineGridDatum.Remove(colorDataColorId);
         EventManager.Raise(CustomEventType.OnLineDeleted,_currentLineColorData.ColorId);
         Debug.Log($"Cleared line {colorDataColorId}");
      }


      private bool AreAdjacent(GridDatum g1, GridDatum g2)
      {
         return Mathf.Abs(g1.PosX - g2.PosX) + Mathf.Abs(g1.PosY - g2.PosY) <= 1;
      }
      
      private bool IsOverlappingCurrentLine(GridDatum grid)
      {
         return _currentLineData
                   .FindIndex((position) => (position.x == grid.PosX) && (position.y == grid.PosY)) > 0;
      }
      
    
      private void ExtendCurrentLine(GridDatum gridDatum, bool isDot)
      {
         var position = GridUtility.GetLocalPositionFromIndices(gridDatum.PosX, gridDatum.PosY);
         _currentGridDatum = gridDatum;
         _currentLineData.Add(position);
         if (!isDot)
         {
            gridDatum.ColorData = _currentLineColorData;
            _currentLineGridData.Add(gridDatum);
         }
         UpdateLine(_currentLineRenderer,_currentLineData);
      }
      
      private void OnDragBeganOnDotHandler(object grid)
      {
         var cellView= (GridCellView)grid;
         _currentDotOrigin = cellView as DotView;
         var gridDatum = cellView.GridDatum;
         if (_lineLookup.ContainsKey(gridDatum.ColorData.ColorId))
         {
            _currentLineRenderer = _lineLookup[gridDatum.ColorData.ColorId];
            if (_completedLines.ContainsKey(gridDatum.ColorData.ColorId))
            {
               ClearLine(gridDatum.ColorData.ColorId);
            }
         }
         else
         {
            _currentLineRenderer = Instantiate(_linePrefab,transform,false);
            _currentLineRenderer.startColor = _currentLineRenderer.endColor = gridDatum.ColorData.UnityColor;
            _lineLookup.Add(gridDatum.ColorData.ColorId,_currentLineRenderer);
         }
         
         var lineData= new List<Vector3Int>();
         var gridData = new List<GridDatum>();
         lineData.Add(GridUtility.GetLocalPositionFromIndices(gridDatum.PosX,gridDatum.PosY));
         UpdateLine(_currentLineRenderer, lineData);
         _currentGridDatum = gridDatum;
         _currentLineColorData = gridDatum.ColorData;
         _currentLineData = lineData;
         _currentLineGridData = gridData;
         _lineDrawState = LineDrawState.Drawing;
      }
      
      private void UpdateLine(LineRenderer lineRenderer, List<Vector3Int> vector3Ints)
      {
         lineRenderer.transform.localPosition = Vector3.zero;
         lineRenderer.SetPositions(vector3Ints);
      }
      
      void OnDisable()
      {
         EventManager.UnregisterHandler(CustomEventType.OnBeginDragOnDot, OnDragBeganOnDotHandler);
         EventManager.UnregisterHandler(CustomEventType.OnGridCellEnter, OnGridCellEnterHandler);
         EventManager.UnregisterHandler(CustomEventType.OnGridCellEndedDrag, OnGridCellEndedDragHandler);
      }
   }
}