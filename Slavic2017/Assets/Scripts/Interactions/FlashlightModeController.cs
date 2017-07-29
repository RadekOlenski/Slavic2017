using DG.Tweening;
using Enums;
using Events;
using FOV;
using Managers;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Interactions
{
    public class FlashlightModeController : MonoBehaviour
    {
        private GameObject player;
        private GameObject playerFOV;
        private GameObject flashlight;

        public LayerMask MouseTargetLayer;
        public int FlashlightUseCost = 2;

        // Use this for initialization
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagsEnum.Player);
            playerFOV = GameObject.FindGameObjectWithTag(TagsEnum.PlayerFOV);
            flashlight = GameObject.FindGameObjectWithTag(TagsEnum.Flashlight);
        }

        private void Start()
        {
            Debug.Log("Flashlight mode enabled");
        }

        // Update is called once per frame
        void Update()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, 100f, MouseTargetLayer))
            {
                Vector3 playerToMouse = floorHit.point;
                playerToMouse.y = playerFOV.transform.position.y;
                playerFOV.transform.LookAt(playerToMouse, Vector3.up);
            }

            if (!Input.GetMouseButtonDown((int) MouseButton.LeftMouse)) return;
            flashlight.transform.DORotateQuaternion(playerFOV.transform.rotation, 1f);
            player.GetComponent<Unit>().ActionPoints -= FlashlightUseCost;
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        }

        private void OnEnable()
        {
            playerFOV.GetComponent<PlayerFieldOfView>().DrawFOV = true;
        }

        private void OnDisable()
        {
            playerFOV.GetComponent<PlayerFieldOfView>().DrawFOV = false;
        }
    }
}