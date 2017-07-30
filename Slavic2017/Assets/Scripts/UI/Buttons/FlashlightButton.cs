using Enums;
using Events;
using Interactions;
using Managers;
using UnityEngine;

namespace UI.Buttons
{
    public class FlashlightButton : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        #region Unity Methods

        // Use this for initialization
        void Start()
        {
            lineRenderer = GameObject.FindGameObjectWithTag(TagsEnum.PathLine).GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            {
                gameObject.SetActive(false);
            }
        }

        //private void OnMouseOver()
        //{
        //    lineRenderer.enabled = false;
        //    EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(false));
        //}

        //private void OnMouseExit()
        //{
        //    lineRenderer.enabled = true;
        //    EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(true));
        //}

        #endregion

        #region Click Methods

        public void EnableFlashlightMode()
        {
            GameObject.FindGameObjectWithTag(TagsEnum.PathLine).GetComponent<LineRendererController>().ClearLine();
            //lineRenderer.enabled = false;
            //EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(false));
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(true));
        }

        #endregion
    }
}