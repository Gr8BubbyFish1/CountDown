using System;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private PasswordWindows[] windows;
    public int passwordLength;
    private bool winWindows;
    private bool loseWindows;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        //StaticManager.setTimer(10);
        //Debug.Log("TimerFireA");
        SetDifficulty();
        foreach (PasswordWindows window in windows)
        {
            window.setPasswordLength(passwordLength);
            window.GeneratePassword();
        }
    }

    private void Update()
    {
        if (windows[0].done && windows[1].done && windows[2].done && windows[3].done && !winWindows)
        {
            winWindows = true;
            //Debug.Log("Game Complete");
            audioManager.PlaySFX(audioManager.winSFX);
            StaticManager.EndMiniGame();
        }

        if (StaticManager.hasTimeRunOut() && !loseWindows)
        {
            loseWindows = true;
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }

    private void SetDifficulty()
    {
        float roundNumber = StaticManager.totalRoundsPlayed;
        
        switch (roundNumber)
        {
            case 0:
                passwordLength = 2;
                StaticManager.setTimer(20);
                break;
            case 1:
                passwordLength = 3;
                StaticManager.setTimer(15);
                break;
            case 2:
                passwordLength = 4;
                StaticManager.setTimer(15);
                break;
            case 3:
                passwordLength = 5;
                StaticManager.setTimer(15);
                break;
            case 4:
                passwordLength = 3;
                StaticManager.setTimer(10);
                break;
            case 5:
                passwordLength = 4;
                StaticManager.setTimer(10);
                break;
            default:
                passwordLength = 5;
                StaticManager.setTimer(10);
                break;
        }
    }
}
