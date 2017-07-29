using Enums;
using Managers;
using UnityEngine;

namespace UI.Panels
{
    public class GameOverPanelController : MonoBehaviour
    {
        public GameObject GameOverPanel;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            {
                GameOverPanel.SetActive(true);
            }
        }
    }
}