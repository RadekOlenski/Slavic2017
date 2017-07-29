using Enums;
using Events;
using Interactions;
using Managers;
using UnityEngine;

namespace UI.Buttons
{
    public class FlashlightButton : MonoBehaviour
    {
        #region Unity Methods

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        #endregion

        #region Click Methods

        public void EnableFlashlightMode()
        {
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(true));
        }

        #endregion
    }
}