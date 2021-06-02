using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayTransitionController : MonoBehaviour
{
    public Animator songNameAndScoreAnimator;

    public Animator transitionAnimator;

    private void Start()
    {
        StartCoroutine(WaitForPlayPantegonAni());
    }

    private IEnumerator WaitForPlayPantegonAni()
    {
        yield return new WaitForSeconds(1.5f);

        songNameAndScoreAnimator.SetTrigger("In");

        StartCoroutine(Conductor.instance.WaitForPlayTime());
    }

    public IEnumerator ExitScene(int sceneID)
    {
        songNameAndScoreAnimator.SetTrigger("Out");

        yield return new WaitForSeconds(1.5f);

        transitionAnimator.SetTrigger("GameEnd");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(sceneID);
    }

}
