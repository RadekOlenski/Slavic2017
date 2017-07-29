using Enums;
using Managers;
using UnityEngine;

namespace UI.Panels
{
    public class ActionPanelController : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            {
                gameObject.SetActive(false);
            }
        }
    }
}