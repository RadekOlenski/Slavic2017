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
        [SerializeField] private MovementModeController movementModeController;

        public Unit PlayerUnit;

        #region Properties

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

        [NonSerialized] private bool movementModeEnabled;

        public bool MovementModeEnabled
        {
            get { return flashlightModeEnabled; }
            set
            {
                movementModeEnabled = value;
                movementModeController.enabled = movementModeEnabled;
            }
        }

        #endregion

        // Use this for initialization
        void Start()
        {
            EventManager.Instance.AddListener<InteractionEvents.EnableFlashlightModeEvent>(HandleFlashlightMode);
            EventManager.Instance.AddListener<InteractionEvents.EnableMovementModeEvent>(HandleMovementMode);
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
            MovementModeEnabled = false;
            Debug.Log("All modes disabled");
        }

        #endregion

        #region Event Handlers

        private void HandleFlashlightMode(InteractionEvents.EnableFlashlightModeEvent e)
        {
            if (PlayerUnit.ActionPoints <= 0 && e.FlashlightModeEnabled) return;
            FlashlightModeEnabled = e.FlashlightModeEnabled;
        }

        private void HandleMovementMode(InteractionEvents.EnableMovementModeEvent e)
        {
            if (PlayerUnit.ActionPoints <= 0 && e.MovementModeEnabled) return;
            MovementModeEnabled = e.MovementModeEnabled;
        }

        private void OnDestroy()
        {
            //EventManager.Instance.RemoveListener<InteractionEvents.EnableFlashlightModeEvent>(HandleFlashlightMode);
        }

        #endregion
    }
}