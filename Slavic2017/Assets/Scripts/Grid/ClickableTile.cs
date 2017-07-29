using UnityEngine;
using System.Collections;
using Enums;
using Grid;

public class ClickableTile : MonoBehaviour
{
    public static bool IsMovement;
    public static bool IsEnabled;
    public int tileX;
    public int tileY;
    public TileMap map;
    private Unit playerUnit;

    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag(TagsEnum.Player).GetComponent<Unit>();
    }

    void OnMouseEnter()
    {
        if (IsMovement || !IsEnabled) return;
        Debug.Log(gameObject.name + " Enter!");
        map.GeneratePathTo(tileX, tileY);
    }

    void OnMouseUp()
    {
        if (!IsEnabled) return;
        Debug.Log("Click!");
        IsMovement = true;
        playerUnit.Move();
    }

}