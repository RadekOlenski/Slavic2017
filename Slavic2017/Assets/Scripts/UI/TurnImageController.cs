using System.Collections;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class TurnImageController : MonoBehaviour
{
    private RawImage turnImage;
    public Texture playerTurn;
    public Texture enemyTurn;

    void Start()
    {
        turnImage = GetComponent<RawImage>();
        EventManager.Instance.AddListener<InteractionEvents.SwitchTurnImageToPlayerEvent>(HandleImage);
    }

    private void HandleImage(InteractionEvents.SwitchTurnImageToPlayerEvent e)
    {
        turnImage.texture = e.IsPlayerTurn ? playerTurn : enemyTurn;
    }
}
