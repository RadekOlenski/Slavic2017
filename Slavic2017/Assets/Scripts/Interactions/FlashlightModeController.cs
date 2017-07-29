﻿using DG.Tweening;
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

        // Use this for initialization
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagsEnum.Player);
            playerFOV = GameObject.FindGameObjectWithTag(TagsEnum.PlayerFOV);
            flashlight = GameObject.FindGameObjectWithTag(TagsEnum.Flashlight);
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(playerFOV.transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            playerFOV.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));

            if (!Input.GetMouseButtonDown((int) MouseButton.LeftMouse)) return;
            flashlight.transform.DORotateQuaternion(playerFOV.transform.rotation, 1f);
            //flashlight.transform.rotation = playerFOV.transform.rotation;
            EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        }

        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
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