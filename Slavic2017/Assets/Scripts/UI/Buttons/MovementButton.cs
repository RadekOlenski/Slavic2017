using Events;
using Managers;
using UnityEngine;

namespace UI.Buttons
{
    public class MovementButton : MonoBehaviour
    {
        #region Click Methods

        public void EnableMovementMode()
        {
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(true));
        }

        #endregion
    }
}