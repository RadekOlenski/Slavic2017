using UnityEngine;
using System.Collections.Generic;
using Enums;
using Events;
using Grid;
using Managers;

public class Unit : MonoBehaviour
{
    [System.NonSerialized]
    public int tileX;
    [System.NonSerialized]
    public int tileY;
    [System.NonSerialized]
    public List<Node> currentPath = null;
    public TileMap map;

    public int ActionPoints = 4;
    private int baseActionPoints;
    private bool isMoving = false;
    private Animator anim;

    void Start()
    {
        baseActionPoints = ActionPoints;
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }
    void Update()
    {
        if (isMoving)
        {
            // Have we moved our visible piece close enough to the target tile that we can
            // advance to the next step in our pathfinding?
            if (Vector3.Distance(transform.position, new Vector3(tileX, transform.position.y, tileY)) < 0.1f)
                MoveNextTile();

            // Smoothly animate towards the correct map tile.
            var target = new Vector3(tileX, transform.position.y, tileY);
            transform.position = Vector3.MoveTowards(transform.position, target, 3f * Time.deltaTime);
            Vector3 targetDir = target - transform.position;
            float step = 5f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        if (ActionPoints <= 0 && gameObject.CompareTag(TagsEnum.Player))
        {
            EventManager.Instance.InvokeEvent(new InteractionEvents.EnableMovementModeEvent(false));
            EventManager.Instance.InvokeEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        }
    }

    public void MoveNextTile()
    {
        if (currentPath == null || ActionPoints <= 0)
        {
            isMoving = false;
            if (anim != null)
            {
                anim.SetBool("isWalking", false);
            }
            ClickableTile.IsMovement = false;
            return;
        }

        transform.position = new Vector3(tileX, transform.position.y, tileY);

        // Get cost from current tile to next tile
        ActionPoints -= (int)map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

        // Move us to the next tile in the sequence
        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        //transform.position = map.TileCoordToWorldCoord(tileX, tileY);   // Update our unity world position

        // Remove the old "current" tile
        currentPath.RemoveAt(0);

        if (currentPath.Count == 1)
        {
            // We only have one tile left in the path, and that tile MUST be our ultimate
            // destination -- and we are standing on it!
            // So let's just clear our pathfinding info.
            currentPath = null;
        }

        // reset path - no multi-turned movement
        //currentPath = null;
    }

    public void RestoreActionPoints()
    {
        ActionPoints = baseActionPoints;
    }

    public void Move()
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }
        isMoving = true;
    }
}