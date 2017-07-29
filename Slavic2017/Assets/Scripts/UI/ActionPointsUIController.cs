using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointsUIController : MonoBehaviour
{
    private Text actionPointsText;
    private Unit playerUnit;

    void Start()
    {
        actionPointsText = GetComponent<Text>();
        playerUnit = GameObject.FindGameObjectWithTag(TagsEnum.Player).GetComponent<Unit>();
    }

    void Update()
    {
        actionPointsText.text = "Action Points: " + playerUnit.ActionPoints;
    }
}
