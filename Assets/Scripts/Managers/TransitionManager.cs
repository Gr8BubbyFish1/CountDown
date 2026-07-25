using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TransitionManager : MonoBehaviour
{
    
    [SerializeField] private PlayableDirector chompUp;
    [SerializeField] private PlayableDirector chompDown;
    [SerializeField] private TextMeshProUGUI splashTitle;
    public String splashTitleText;

    private void Start()
    {
        StaticManager.transitionManager = this;
        splashTitle.text = splashTitleText;

        if (StaticManager.skipNextIntro)
        {
            StaticManager.skipNextIntro = false;
        }
        else
        {
            OpenScene();
        }
    }

    public void OpenScene()
    {
        StartCoroutine(OpenTransition());
    }

    public void CloseScene(String goToSceneName)
    {
        StartCoroutine(CloseTransition(goToSceneName));
    }
    
    private IEnumerator OpenTransition()
    {
        StaticManager.transitionPlaying = true;
        
        chompUp.Play();
        while (chompUp.time < chompUp.duration)
            yield return null;

        StaticManager.transitionPlaying = false;
    }
    
    private IEnumerator CloseTransition(String goToSceneName)
    {
        StaticManager.transitionPlaying = true;

        chompDown.Play();
        while (chompDown.time < chompDown.duration)
            yield return null;
        
        StaticManager.transitionPlaying = false;
        Debug.Log("2");
        StaticManager.GoToScene(goToSceneName);
    }

}
