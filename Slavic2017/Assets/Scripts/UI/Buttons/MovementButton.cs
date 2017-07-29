using Enums;
using Events;
using Managers;
using UnityEngine;

namespace UI.Buttons
{
    public class MovementButton : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            {
                gameObject.SetActive(false);
            }
        }

        #region Click Methods

        public void EnableMovementMode()
        {
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(true));
        }

        #endregion
    }
}