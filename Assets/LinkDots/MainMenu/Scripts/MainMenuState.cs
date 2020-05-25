using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : MonoBehaviour
{
    [SerializeField] private Button startMenu;
    [SerializeField] private Button exitMenu;

    private StateManager _stateManger;
    void Awake()
    {
        _stateManger = FindObjectOfType<StateManager>();
    }
    void OnEnable()
    {
        startMenu.onClick.AddListener(OnStartClicked);
        exitMenu.onClick.AddListener(OnExitClicked);
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    private void OnStartClicked()
    {
        EventManager.Raise(CustomEventType.OnLevelSelectionOpened);
    }

    void OnDisable()
    {
        startMenu.onClick.RemoveListener(OnStartClicked);
        exitMenu.onClick.RemoveListener(OnExitClicked);
    }
}
