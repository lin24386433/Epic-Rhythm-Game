using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GamePlayTransitionController transitionCtrl;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject countDownObj;

    bool isCountDownOver;

    [SerializeField]
    private Button retryBtn;

    [SerializeField]
    private Button resumeBtn;

    [SerializeField]
    private Button exitBtn;

    private void Start()
    {
        retryBtn.onClick.AddListener(OnRetryBtnClicked);

        resumeBtn.onClick.AddListener(OnResumeBtnClicked);

        exitBtn.onClick.AddListener(OnExitBtnClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GamePlayController.instance.isPaused && Conductor.instance.songAudioSource.isPlaying) 
            { 
                Time.timeScale = 0f;

                Conductor.instance.songAudioSource.Pause();

                pausePanel.SetActive(true);

                GamePlayController.instance.isPaused = true;
            }
            else
            {
                
            }
        }

        if (countDownObj.activeSelf)
        {
            if (countDownObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && isCountDownOver)
            {
                isCountDownOver = false;

                Time.timeScale = 1f;

                pauseMenu.SetActive(true);

                countDownObj.SetActive(false);

                pausePanel.SetActive(false);

                Conductor.instance.songAudioSource.Play();

                GamePlayController.instance.isPaused = false;
            }
        }
    }

    private void OnRetryBtnClicked() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }

    private void OnResumeBtnClicked()
    {
        pauseMenu.SetActive(false);

        countDownObj.SetActive(true);

        isCountDownOver = true;

    }

    private void OnExitBtnClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

}
