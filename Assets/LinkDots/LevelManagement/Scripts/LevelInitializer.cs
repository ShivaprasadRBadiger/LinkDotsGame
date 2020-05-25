using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;

namespace LinkDots
{
    public class LevelInitializer : MonoBehaviour
    {
        private LevelDataModel _levelData;
        [SerializeField] 
        private DotView dotPrefab;
        [SerializeField] 
        private GridCellView gridCellPrefab;

        void Awake()
        {
            EventManager.RegisterHandler(CustomEventType.OnLoadLevel,OnLoadLevelHandler);
        }
        void OnDestroy()
        {
            EventManager.UnregisterHandler(CustomEventType.OnLoadLevel,OnLoadLevelHandler);
        }
        private void OnLoadLevelHandler(object obj)
        {
            int levelIndex= (int)obj;
            InitializeLevel(levelIndex);
        }

        public void InitializeLevel(int level)
        {
           if (LoadLevelData(level)) return;
           PopulateCells(_levelData);
           PopulateDots(_levelData);
        }

        private bool LoadLevelData(int level)
        {
            var levelDataFile = Resources.Load<TextAsset>($"LevelData/Level{level}");
            var jsonData = levelDataFile.text;
            Debug.Log($"Level Data : {jsonData}");
            try
            {
                _levelData = JsonUtility.FromJson<LevelDataModel>(jsonData);
                EventManager.Raise(CustomEventType.OnLevelDataLoaded,_levelData);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to parse level data! Please verify level data is in right format!");
                Debug.LogException(e);
                return true;
            }

            return false;
        }

        private void PopulateDots(LevelDataModel levelData)
        {
            foreach (var gridDatum in levelData.GridData)
            {
                var position=   GridUtility.GetLocalPositionFromIndices(gridDatum.PosX, gridDatum.PosY,levelData.GridSize);
                var cell = Instantiate(dotPrefab, transform);
                cell.transform.localPosition = position;
                cell.transform.localPosition += Vector3.back * 0.01f;
                cell.Initialize(gridDatum);
            }
        }

        private void PopulateCells(LevelDataModel levelData)
        {
            for (int i = 0; i < levelData.GridSize; i++)
            {
                for (int j = 0; j < levelData.GridSize; j++)
                {
                    var position=  GridUtility.GetLocalPositionFromIndices(i, j,levelData.GridSize);
                    var cell = Instantiate(gridCellPrefab, transform);
                    cell.transform.localPosition = position;
                    var gridDatum = new GridDatum(i, j);
                    cell.Initialize(gridDatum);
                }
            }
        }
    }

    public static class GridUtility
    {
        public static Vector3Int GetLocalPositionFromIndices(int posX, int posY , int gridSize= 5)
        {
            if (gridSize > 0) return new Vector3Int(posX, posY, 0);
            
            Debug.LogError($"Invalid GridSize {gridSize}.");
            return Vector3Int.zero;
        }
    }
}

