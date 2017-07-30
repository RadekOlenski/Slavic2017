using Enums;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class GameOverPanelController : MonoBehaviour
    {
        public GameObject GameOverPanel;
        public Texture girlImage;
        public Texture enemyImage;
        private Text text;
        private RawImage rawImage;
        public static bool IsWon;

        void Start()
        {
            text = GameOverPanel.GetComponentInChildren<Text>();
            rawImage = GameOverPanel.GetComponentInChildren<RawImage>();
        }

        void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            {
                GameOverPanel.SetActive(true);
                if (IsWon)
                {
                    rawImage.texture = girlImage;
                    text.text = "You won!";
                }
                else
                {
                    rawImage.texture = enemyImage;
                    text.text = "You DIED!";
                }
            }
        }
    }
}