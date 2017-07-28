using System;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Interactions
{
    public class InteractionModeController : MonoBehaviour
    {
        [SerializeField] private FlashlightModeController flashlightModeController;

        //[SerializeField]
        //private MovementModeController movementModeController;

        [NonSerialized] private bool flashlightModeEnabled;

        public bool FlashlightModeEnabled
        {
            get { return flashlightModeEnabled; }
            set
            {
                flashlightModeEnabled = value;
                flashlightModeController.enabled = flashlightModeEnabled;
            }
        }

        [NonSerialized] public bool MovementModeEnabled;

        // Use this for initialization
        void Start()
        {
            EventManager.Instance.AddListener<InteractionEvents.EnableFlashlightModeEvent>(HandleFlashlightMode);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown((int) MouseButton.RightMouse))
            {
                DisableInteractionModes();
            }
        }

        #region Private Methods

        private void DisableInteractionModes()
        {
            FlashlightModeEnabled = false;
        }

        #endregion

        #region Event Handlers

        private void HandleFlashlightMode(InteractionEvents.EnableFlashlightModeEvent e)
        {
            FlashlightModeEnabled = e.FlashlightModeEnabled;
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<InteractionEvents.EnableFlashlightModeEvent>(HandleFlashlightMode);
        }

        #endregion
    }
}