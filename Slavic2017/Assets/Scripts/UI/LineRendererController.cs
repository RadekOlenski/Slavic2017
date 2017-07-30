using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private Unit playerUnit;
    private LineRenderer line;

    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag(TagsEnum.Player).GetComponent<Unit>();
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (playerUnit.currentPath != null)
        {
            line.enabled = true;
            //int currNode = 0;
            line.positionCount = playerUnit.currentPath.Count;
            //Vector3 [] positions = new Vector3[line.positionCount];

            List<Vector3> positionList = new List<Vector3>();
            foreach (var node in playerUnit.currentPath)
            {
                positionList.Add(new Vector3(node.x, .6f, node.y));
            }
            line.SetPositions(positionList.ToArray());
            //while (currNode < playerUnit.currentPath.Count - 1)
            //{
            //    Vector3 start = new Vector3(playerUnit.currentPath[currNode].x,0, playerUnit.currentPath[currNode].y) +
            //                    new Vector3(0, .6f, 0f);
            //    Vector3 end = new Vector3(playerUnit.currentPath[currNode + 1].x,0, playerUnit.currentPath[currNode + 1].y) +
            //                  new Vector3(0, .6f, 0f);
            //    //Debug.DrawLine(start, end, Color.red);
            //    currNode++;
            //}
        }
        else
        {
            line.enabled = false;
        }
    }

    public void ClearLine()
    {
        playerUnit.currentPath = null;
        line.enabled = false;
    }
}
