using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public static class StaticManager
{
    //Multi run stats
    public static int highScore;
    
    //Single Run Stats
    public static int totalGamesPlayed;
    public static int roundNumber; // out of 5
    public static int lives;
    public static int totalRoundsPlayed;
    public static String notificationNote;
    
    //Flags
    public static bool transitionPlaying;
    public static bool skipNextIntro;
    
    //Managers
    public static TransitionManager transitionManager;
    public static SideBarsManager sideBarsManager;
    //public static BetweenGameManager betweenGameManager;
    
    //Static Manager Only
    private static List<String> miniGameList;
    private static List<String> miniGameQueue;
    
    
    //Executes on Game Start
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGame()
    {
        miniGameList = new List<String>(5);//update with # of minigames
        //Add minigames here
        miniGameList.Add("Window Scene 1");
        miniGameList.Add("Window Scene 2");
        miniGameList.Add("Window Scene 3");
        miniGameList.Add("Window Scene 4");
        miniGameList.Add("Window Scene 5");
        
        skipNextIntro = true;
    }

    public static void RestartGame()
    { 
        totalGamesPlayed = 0; 
        roundNumber = 1;
        lives = 3; 
        totalRoundsPlayed = 0;
        notificationNote = "";
        
        GeneratePlayList();
        transitionManager.CloseScene("Between Game Screen");
    }

    public static String GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void EndMiniGame()
    {
        if (lives > 0)
        {

            roundNumber++;
            totalGamesPlayed++;
            if (roundNumber == 6)
            {
                totalRoundsPlayed++;
                notificationNote = "Challenge Increasing...";
                roundNumber = 1;
            }
            else
            {
                notificationNote = "";
            }

            try
            {
                transitionManager.CloseScene("Between Game Screen");
            }
            catch
            {
                GoToScene("Between Game Screen");
                Debug.Log("No transition detected");
            }
        }
        else
        {
            CompletelyEndGame();
        }
    }

    public static void GoToScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public static void GoToScene(String sceneName, bool skipIntro)
    {
        skipNextIntro = skipIntro;
        SceneManager.LoadScene(sceneName);
    }

    public static void StartNextGame()
    {
        if (roundNumber == 1)
        {
            GeneratePlayList();
            GoToScene(miniGameQueue[roundNumber-1]);
        }
        else
        {
            GoToScene(miniGameQueue[roundNumber-1]);
        }
    }

    public static void CompletelyEndGame()
    {
        transitionManager.CloseScene("Title Screen");
    }
    
    public static bool hasTimeRunOut()
    {
            return sideBarsManager.hasTimeRunOut();
    }
    
    public static float getCurrentTime()
    {
        return sideBarsManager.getCurrentTime();
    }

    public static void setTimer(float time)
    {
        sideBarsManager.setMaxTime(time);
    }

    public static void removeLife()
    {
        lives--;
        sideBarsManager.UpdateSideBars();
    }

    public static void removeLife(int number)
    {
        lives -= number;
        sideBarsManager.UpdateSideBars();
    }

    private static void GeneratePlayList()
    {
        for (int i = 0; i < miniGameList.Count; i++)
        {
            int randomIndex = Random.Range(0, miniGameList.Count);
            (miniGameList[i], miniGameList[randomIndex]) = (miniGameList[randomIndex], miniGameList[i]);
        }

        miniGameQueue = new List<String>();
        for (int i = 0; i < 5; i++)
        {
            miniGameQueue.Add(miniGameList[i]);
        }
    }
    
}
