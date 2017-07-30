using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Events;
using Grid;
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
        if (isEnemyTurn)
        {
            if (enemies.Any(e => Mathf.Abs(e.tileX - player.tileX) < 0.3f && Mathf.Abs(e.tileY - player.tileY) < 0.3f))
            {
                isEnemyTurn = false;
                EventManager.Instance.QueueEvent(new GameOverEvent(false));
                return;
            }
            if (!enemies.Any(e => e.ActionPoints > 0))
            {
                isEnemyTurn = false;
                HandlePlayerTurn();
            }
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
        isEnemyTurn = true;
        HandleEnemyTurn();
    }

    void HandlePlayerTurn()
    {
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(true));
        //EventManager.Instance.QueueEvent(new InteractionEvents.SwitchTurnImageToPlayerEvent(true));
        player.RestoreActionPoints();
        map.SelectedUnit = player.gameObject;
        map.SetupSelectedUnit();
    }

    void HandleEnemyTurn()
    {
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableFlashlightModeEvent(false));
        //EventManager.Instance.QueueEvent(new InteractionEvents.EnableMovementModeEvent(false));
        //EventManager.Instance.QueueEvent(new InteractionEvents.SwitchTurnImageToPlayerEvent(false));
        foreach (var enemy in enemies)
        {
            enemy.RestoreActionPoints();
            map.SelectedUnit = enemy.gameObject;
            map.SetupSelectedUnit();
            map.GeneratePathTo((int)player.tileX, (int)player.tileY);
            enemy.Move();
        }
        //HandlePlayerTurn();
    }
}
