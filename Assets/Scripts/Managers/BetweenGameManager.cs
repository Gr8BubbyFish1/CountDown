using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class BetweenGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gamesDone;
    [SerializeField] private TextMeshProUGUI roundsDone;
    [SerializeField] private TextMeshProUGUI currentRound;
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI notification;
    
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayableDirector fadeOut;
    private void Start()
    {
        //StaticManager.betweenGameManager = this;
        UpdateText();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !StaticManager.transitionPlaying)
        {
            StartCoroutine(TransitionOut());
        }
    }

    private IEnumerator TransitionOut()
    {
        StaticManager.transitionPlaying = true;
        
        fadeOut.Play();
        while (fadeOut.time < fadeOut.duration)
            yield return null;

        StaticManager.transitionPlaying = false;
        StaticManager.StartNextGame();
    }

    public void UpdateText()
    {
        gamesDone.text = "Total Games Completed: " + StaticManager.totalGamesPlayed;
        roundsDone.text = "Total Rounds Completed: " + StaticManager.totalRoundsPlayed;
        currentRound.text = "Round " + StaticManager.roundNumber + " / 5";
        lives.text = "Lives Remaining: " + StaticManager.lives;
        notification.text = StaticManager.notificationNote;
        if (notification.text.Equals("Challenge Increasing..."))
        {
            audioManager = StaticManager.audioManager;
            audioManager.PlaySFX(audioManager.speedUp);
        }
    }
}
