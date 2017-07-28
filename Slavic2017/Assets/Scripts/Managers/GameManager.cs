using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public struct LoadedLevelInfo
        {
            public string SceneName { get; set; }
            public int SceneIndex { get; set; }
        }

        public GameState CurrentGameState { get; set; }

        public LoadedLevelInfo LastLoadedLevel;

        private EventManager eventManager;

        #region Constructor

        void Start()
        {
            eventManager = EventManager.Instance;

            //We set up listener for LoadSceneEvents. It allows GameManager to change scene on event invoke
            eventManager.AddListener<LoadSceneEvent>(LoadScene);
            eventManager.AddListener<LoadSceneByIndexEvent>(LoadSceneByIndex);
            eventManager.AddListener<ChangeGameStateEvent>(ChangeGameState);
            eventManager.AddListener<GameOverEvent>(EndGame);
            eventManager.AddListener<QuitGameEvent>(QuitGame);
        }

        #endregion

        #region Private Methods

        private void LoadScene(LoadSceneEvent sceneEvent)
        {
            SceneManager.LoadSceneAsync(sceneEvent.SceneToLoad);
        } 

        private void LoadSceneByIndex(LoadSceneByIndexEvent sceneEvent)
        {
            SceneManager.LoadSceneAsync(sceneEvent.SceneIndex);
        }

        private void ChangeGameState(ChangeGameStateEvent gameStateEvent)
        {
            CurrentGameState = gameStateEvent.CurrentGameState;
            if (CurrentGameState.Equals(GameState.Paused)) PauseGame();
            if (CurrentGameState.Equals(GameState.InGame)) ResumeGame();
        }

        private void ResumeGame()
        {
            Time.timeScale = 1.0f;
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void EndGame(GameOverEvent e)
        {
            LastLoadedLevel = new LoadedLevelInfo();
            LastLoadedLevel.SceneIndex = SceneManager.GetActiveScene().buildIndex;
            LastLoadedLevel.SceneName = SceneManager.GetActiveScene().name;
        }

        private void QuitGame(QuitGameEvent e)
        {
            Application.Quit();
        }
		
        #endregion

        #region Singletton Pattern
        private void Awake()
        {
            if (instance == null)

                instance = this;

            else if (instance != this)

                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = new GameManager();
                DontDestroyOnLoad(instance);
                return instance;
            }
        }
        #endregion
    }
}