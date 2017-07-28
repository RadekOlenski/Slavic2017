using Enums;
using Events;
using Managers;
using UnityEngine;

namespace Scenes
{
    //All scripts for Intro scene actions
    public class Intro : MonoBehaviour
    {
        private EventManager eventManager;
        //Game manager MUST be instantiated here
        private GameManager gameManager;

        private void Awake()
        {
            eventManager = EventManager.Instance;
            gameManager = GameManager.Instance;
            eventManager.InvokeEvent(new ChangeGameStateEvent(GameState.Intro));
        }

        private void Start()
        {
            //We invoke event to change scene for Main Menu
            eventManager.InvokeEvent(new ChangeGameStateEvent(GameState.MainMenu));
            eventManager.InvokeEvent(new LoadSceneEvent(ScenesEnum.Menu));
        }
    }
}