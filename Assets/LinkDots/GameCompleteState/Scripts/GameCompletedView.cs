using System;
using System.ComponentModel;
using EventSystem;
using LinkDots;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


   public class GameCompletedView : MonoBehaviour
   {
      public enum CompletionState
      {
         Won,
         Lost
      }
   
      [SerializeField]
      Settings settings;

      private int _totalLinesToBeCompleted;
      private int _totalLineCompleted;

      private void Awake()
      {
         settings.exitToMenu.onClick.AddListener(OnGameRestartClicked);
         EventManager.RegisterHandler(CustomEventType.OnLevelDataLoaded,OnLevelDataLoadedHandler);
         EventManager.RegisterHandler(CustomEventType.OnLineDeleted,OnLineDeletedHandler);
         EventManager.RegisterHandler(CustomEventType.OnLineCompleted,OnLineCompletedHandler);
      }

      private void OnDestroy()
      {
         EventManager.UnregisterHandler(CustomEventType.OnLevelDataLoaded,OnLevelDataLoadedHandler);
         EventManager.UnregisterHandler(CustomEventType.OnLineDeleted,OnLineDeletedHandler);
         EventManager.UnregisterHandler(CustomEventType.OnLineCompleted,OnLineCompletedHandler);
      }

      private void OnLineDeletedHandler(object obj)
      {
         Debug.Log("Line Deletion registered");
         _totalLineCompleted--;
      }

      private void OnLevelDataLoadedHandler(object obj)
      {
         LevelDataModel levelData = (LevelDataModel) obj;
         _totalLinesToBeCompleted = levelData.GridData.Count / 2;
      }

      private void ShowWonScreen()
      {
         Initialize(CompletionState.Won);
      }
      private void OnLineCompletedHandler(object obj)
      {
         Debug.Log($"Lines complete!{_totalLineCompleted} ");
         _totalLineCompleted++;
         if(_totalLinesToBeCompleted== _totalLineCompleted)
            ShowWonScreen();
      }

      private void OnGameRestartClicked()
      {
         SceneManager.LoadScene(0);
      }

      public void Initialize(CompletionState state)
      {
         switch (state)
         {
            case CompletionState.Won:
               settings.titleText.text = "LEVEL COMPLETE!";
               break;
            default:
               settings.titleText.text = "UNKNOWN OUTCOME!";
               Debug.LogError($"Unhandled state{state}");
               break;
         }
         gameObject.GetComponent<Canvas>().enabled = true;
      }

      [Serializable]
      public struct Settings
      {
         public TextMeshProUGUI titleText;
         public Button exitToMenu;
      }
   }
