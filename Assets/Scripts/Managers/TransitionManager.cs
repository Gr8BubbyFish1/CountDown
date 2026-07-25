using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TransitionManager : MonoBehaviour
{
    
    [SerializeField] private PlayableDirector chompUp;
    [SerializeField] private PlayableDirector chompDown;

    private void Start()
    {
        StaticManager.transitionManager = this;

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
        while (chompDown.time < chompUp.duration)
            yield return null;
        
        StaticManager.transitionPlaying = false;
        StaticManager.GoToScene(goToSceneName);
    }

}
