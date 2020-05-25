using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    [SerializeField] private Settings settings;
    private GameObject _currentState;
    void Awake()
    {
        _currentState = settings.states[0];
        EventManager.RegisterHandler(CustomEventType.OnLevelSelectionOpened,LevelSelectState);
        EventManager.RegisterHandler(CustomEventType.OnLoadLevel,OnLoadLevelHandler);
    }

    private void OnLoadLevelHandler(object obj)
    {
        SwitchState(2);
    }

    private void LevelSelectState(object obj)
    {
        SwitchState(1);
    }

    private void SwitchState(int stateID)
    {
        Debug.Log($"Switching state{stateID}");
        _currentState?.SetActive(false);
        _currentState = settings.states[stateID];
        _currentState?.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentState == settings.states[0])
            {
                settings.quitState.SetActive(true);
            }
            else
            {
                SwitchState(0);
            }
        }
    }

    void  OnDestroy()
    {
        EventManager.UnregisterHandler(CustomEventType.OnLevelSelectionOpened,LevelSelectState);
        EventManager.UnregisterHandler(CustomEventType.OnLoadLevel,OnLoadLevelHandler);
    }

    [Serializable]
    public class Settings
    {
        public List<GameObject> states;
        public GameObject quitState;
    }
}
