using System;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class ScrollEntry : MonoBehaviour
{
    [SerializeField] private Settings settings;

    void OnEnable()
    {
        settings.scrollEntryButton.onClick.AddListener(OnButtonPressed);
    }
    void OnDisable()
    {
        settings.scrollEntryButton.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        EventManager.Raise(CustomEventType.OnLoadLevel,transform.GetSiblingIndex()+1);
    }

    [Serializable]
    public class Settings
    {
        public Button scrollEntryButton;
    }
}
