using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] GameObject credits;
    bool isCreditsActive = false;
    
    public void SendToScene(string sceneName)
    {
        Debug.Log("Sending to scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame()
    {
        StaticManager.RestartGame();
    }

    public void Credits()
    {
        isCreditsActive = !isCreditsActive;
        credits.SetActive(isCreditsActive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
