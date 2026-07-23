using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    
    [SerializeField] private String spring = "hello";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(spring);
    }
    
    public void SendToScene(string sceneName)
    {
        Debug.Log("Sending to scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    
}
