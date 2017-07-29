using UnityEngine;
using System.Collections;
using Enums;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;
    private Unit playerUnit;

    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag(TagsEnum.PlayerTag).GetComponent<Unit>();
    }

    void OnMouseEnter()
    {
        Debug.Log(gameObject.name + " Enter!");
        map.GeneratePathTo(tileX, tileY);
    }

    void OnMouseUp()
    {
        Debug.Log("Click!");
        //map.GeneratePathTo(tileX, tileY);
        playerUnit.MoveNextTile();
    }

}