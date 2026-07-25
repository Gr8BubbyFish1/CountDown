using System;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private PasswordWindows[] windows;
    public int passwordLength;
    private bool winWindows;
    private bool loseWindows;

    private void Start()
    {
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
            StaticManager.EndMiniGame();
        }

        if (StaticManager.hasTimeRunOut() && !loseWindows)
        {
            loseWindows = true;
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }
}
