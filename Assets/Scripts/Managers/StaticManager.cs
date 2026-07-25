using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public static class StaticManager
{
    /**
     * SETUP:
     * Add Minigame Scenes by name under InitializeGame()
     * Add ChompTransitions and SideBars prefabs to scene hierarchy (game can still progress
     * without them but some methods might break)
     *
     * METHODS: Access with StaticManager.Method()
     * RestartGame() - Resets lives and most counters, sends player to a Between Game Scene, full reset state
     * GetCurrentSceneName() - Gets the current scene name (it's in the name lol)
     * EndMiniGame() - Iterates counters and starts the out transition, sends player to Between Game Scene
     *                 If player is out of lives then CompletelyEndGame() is triggered
     * StartNextGame() - Send the player to the next minigame based in the queue based on round number
     *                  (note: does not iterate any counts)
     * CompletelyEndGame() - Lose condition, for now just sends player to main menu
     * GoToScene() - Go to a scene based on the (String) name
     *              (Has an override (bool) skip opening transition)
     * GetCurrentSceneName() - Gets the current scene same as a String
     * hasTimeRunOut() - true if time has run out (requires SidBars to be in the scene)
     * getCurrentTime() - returns the current time left in the timer (requires SidBars to be in the scene)
     * setTimer() - set the starting amount of time in the timer (requires SidBars to be in the scene)
     * removeLife() - remove a life
     *                (Has an override (int) to remove mutable lives)
     * 
     */
    
    //Multi run stats
    public static int highScore;
    
    //Between minigame report
    public static int totalGamesPlayed;
    public static int roundNumber; // number 1 to 5 inclusive
    public static int lives;// defaults to 3
    public static int totalRoundsPlayed; // iterates after every 5 minigames
    public static String notificationNote; // the "Challenge increased" message, but can be set to anything
    
    //Flags
    public static bool transitionPlaying; // useful for preventing actions during a transition
    public static bool skipNextIntro; // skips the open transition of the next scene loaded if true, resets to false after
    
    //Managers (set if they exist as part of ChompTransitions and SideBars)
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
        miniGameList.Add("RiceScene");
        miniGameList.Add("Window Scene 2");
        miniGameList.Add("Window Scene 3");
        miniGameList.Add("Window Scene 4");
        miniGameList.Add("Window Scene 5");

        if (SceneManager.GetActiveScene().name.Equals("Title Screen"))
        {
            skipNextIntro = true;
        }
        else // we are debugging a scene
        {
            Debug.Log("0");
            totalGamesPlayed = 0; 
            roundNumber = 1;
            lives = 3; 
            totalRoundsPlayed = 0;
            notificationNote = "";
            GeneratePlayList();
        }
        
    }

    public static void RestartGame()
    { 
        totalGamesPlayed = 0; 
        roundNumber = 1;
        lives = 3; 
        totalRoundsPlayed = 0;
        notificationNote = "";
        
        GeneratePlayList();
        try
        {
            transitionManager.CloseScene("Between Game Screen");
        }
        catch
        {
            GoToScene("Between Game Screen");
        }
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
                Debug.Log("1");
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
        try
        {
            transitionManager.CloseScene("Title Screen");
        }
        catch
        {
            GoToScene("Title Screen");
        }
    }
    
    public static void GoToScene(String sceneName)
    {
        Debug.Log("3");
        SceneManager.LoadScene(sceneName);
    }
    
    public static void GoToScene(String sceneName, bool skipIntro)
    {
        skipNextIntro = skipIntro;
        SceneManager.LoadScene(sceneName);
    }
    
    public static String GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
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
