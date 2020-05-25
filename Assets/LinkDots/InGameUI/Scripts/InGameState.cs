using System.Collections;
using System.Collections.Generic;
using EventSystem;
using LinkDots;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moves;
    [SerializeField] private TextMeshProUGUI best;
    private int _moves = 0;

    void Awake()
    {
        EventManager.RegisterHandler(CustomEventType.OnLevelDataLoaded,OnLevelLoadedHandler);
    }

    void OnDestroy()
    {
        EventManager.UnregisterHandler(CustomEventType.OnLevelDataLoaded,OnLevelLoadedHandler);
    }
    void OnEnable()
    {
        EventManager.RegisterHandler(CustomEventType.OnBeginDragOnDot,OnMoveMadeHandler);
    }

    private void OnMoveMadeHandler(object obj)
    {
        _moves++;
        moves.text = $"Moves: {_moves}";
    }

    private void OnLevelLoadedHandler(object obj)
    {
        Debug.Log("Updating loaded level");
        LevelDataModel levelDataModel = (LevelDataModel) obj;
        best.text = $"Best: {levelDataModel.GridData.Count/2}";
    }

    void OnDiable()
    {
        EventManager.UnregisterHandler(CustomEventType.OnBeginDragOnDot,OnMoveMadeHandler);
    }
   
}
