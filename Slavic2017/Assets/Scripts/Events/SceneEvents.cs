using System.Collections.Generic;
using Enums;

namespace Events
{
    //Script for creating event classes associated with Scenes
    public class LoadSceneEvent : GameEvent
    {
        //Event responsible for passing scene to load
        public string SceneToLoad { get; private set; }

        public LoadSceneEvent(string sceneToLoad)
        {
            SceneToLoad = sceneToLoad;
            args = new List<object> {SceneToLoad};
        }

    }

    public class LoadSceneByIndexEvent : GameEvent
    {
        //Event responsible for passing scene to load
        public int SceneIndex { get; private set; }

        public LoadSceneByIndexEvent(int sceneIndex)
        {
            SceneIndex = sceneIndex;
            args = new List<object> {SceneIndex};
        }

    }

    public class ChangeGameStateEvent : GameEvent
    {
        public GameState CurrentGameState { get; private set; }

        public ChangeGameStateEvent(GameState currentGameState)
        {
            CurrentGameState = currentGameState;
            args = new List<object> {CurrentGameState};
        }
    }
}