using System;
using Enums;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class NextLevelButton : MonoBehaviour
    {
        private Button button;

        // Use this for initialization
        void Start()
        {
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(GoToNextLevel);
        }

        private void GoToNextLevel()
        {
            try
            {
                int sceneIndex = GameManager.Instance.LastLoadedLevel.SceneIndex + 1;

                EventManager.Instance.InvokeEvent(new ChangeGameStateEvent(GameState.InGame));

                EventManager.Instance.InvokeEvent(new LoadSceneByIndexEvent(sceneIndex));
            }
            catch (Exception)
            {
                Debug.Log("No more scenes!");
            }
            //EventManager.Instance.InvokeEvent(new PlaySimpleSoundEvent(SoundsEnum.MenuButtonClick));
        }
    }
}