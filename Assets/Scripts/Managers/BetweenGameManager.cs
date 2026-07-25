using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetweenGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gamesDone;
    [SerializeField] private TextMeshProUGUI currentRound;
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI notification;
    private void Start()
    {
        //StaticManager.betweenGameManager = this;
        UpdateText();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StaticManager.StartNextGame();
        }
    }

    public void UpdateText()
    {
        gamesDone.text = "Total Games Completed: " + StaticManager.totalGamesPlayed;
        currentRound.text = "Round" + StaticManager.roundNumber + " / 5";
        lives.text = "Lives Remaining: " + StaticManager.lives;
        notification.text = StaticManager.notificationNote;
    }
}
