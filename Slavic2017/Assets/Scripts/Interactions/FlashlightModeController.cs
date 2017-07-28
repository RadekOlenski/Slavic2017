using Enums;
using FOV;
using UnityEngine;

namespace Interactions
{
    public class FlashlightModeController : MonoBehaviour
    {
        private GameObject player;
        private GameObject playerFOV;
        
        // Use this for initialization
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagsEnum.Player);
            playerFOV = GameObject.FindGameObjectWithTag(TagsEnum.PlayerFOV);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnEnable()
        {
            playerFOV.GetComponent<PlayerFieldOfView>().enabled = true;
        }

        private void OnDisable()
        {
            playerFOV.GetComponent<PlayerFieldOfView>().enabled = false;
        }
    }
}