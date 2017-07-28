using System;
using System.Text.RegularExpressions;
using Enums;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class LevelButton : MonoBehaviour {

        private Button button;
        private EventManager eventManager;

        // Use this for initialization
        private void Start()
        {
            eventManager = EventManager.Instance;
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(LoadLevel);
        }

        private void LoadLevel()
        {
            string buttonText = button.GetComponentInChildren<Text>().text;
            int sceneNumber = Int32.Parse(Regex.Match(buttonText, @"\d+").Value);
            eventManager.InvokeEvent(new ChangeGameStateEvent(GameState.InGame));
            eventManager.InvokeEvent(new LoadSceneEvent("Level_0" + sceneNumber));
            //Uncomment with sounds
            //EventManager.Instance.InvokeEvent(new PlaySimpleSoundEvent(SoundsEnum.MenuButtonClick));
        }
    }
}
