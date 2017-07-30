using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject target;
    private Vector3 offset;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(TagsEnum.Player);
        offset = gameObject.transform.position - target.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = target.transform.position + offset;
    }
}
