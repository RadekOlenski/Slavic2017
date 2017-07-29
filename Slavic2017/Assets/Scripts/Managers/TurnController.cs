using System.Collections;
using System.Collections.Generic;
using Enums;
using Events;
using Managers;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public TileMap map;
    private Unit player;
    private List<Unit> enemies;
    private Unit currentEnemy;
    private bool isEnemyTurn = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsEnum.Player).GetComponent<Unit>();
        enemies = new List<Unit>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag(TagsEnum.Enemy))
        {
            enemies.Add(enemy.GetComponent<Unit>());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //RestoreActionPoints();
        }
    }

    public void EndTurn()
    {
        //isPlayerTurn = !isPlayerTurn;
        //if (isPlayerTurn)
        //{
        //    HandlePlayerTurn();
        //}
        //else
        //{
        //    HandleEnemyTurn();
        //}
        HandleEnemyTurn();
    }

    void HandlePlayerTurn()
    {
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(true));
        player.RestoreActionPoints();
        map.SelectedUnit = player.gameObject;
        map.SetupSelectedUnit();
    }

    void HandleEnemyTurn()
    {
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(false));
        foreach (var enemy in enemies)
        {
            enemy.RestoreActionPoints();
            map.SelectedUnit = enemy.gameObject;
            map.SetupSelectedUnit();
            map.GeneratePathTo((int)player.tileX, (int)player.tileY);
            enemy.Move();
        }
        HandlePlayerTurn();
    }
}
