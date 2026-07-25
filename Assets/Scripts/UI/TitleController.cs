using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    
    public void SendToScene(string sceneName)
    {
        Debug.Log("Sending to scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
}
