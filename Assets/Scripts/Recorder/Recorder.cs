using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    [SerializeField]
    private bool isPlaying = true;

    [SerializeField]
    private Text beatNowTxt;

    [SerializeField]
    private Slider songPlaySlider;

    [SerializeField]
    private Button pauseStartBtn;

    public GameObject noteDetailWindow;

    [SerializeField]
    private Sprite[] pauseStartSprites;

    private void Start()
    {
        pauseStartBtn.onClick.AddListener(OnPauseStartBtnClicked);

        songPlaySlider.onValueChanged.AddListener((delegate { OnSliderValueChanged(); }));

        noteDetailWindow.SetActive(false);

        RecordConductor.instance.songAudioSource.Play();
        RecordConductor.instance.songAudioSource.Pause();

    }

    private void Update()
    {
        UIUpdate();

        if(RecordConductor.instance.songAudioSource.time == RecordConductor.instance.songAudioSource.clip.length)
        {
            RecordConductor.instance.songAudioSource.Stop();
            isPlaying = false;
            pauseStartBtn.image.sprite = pauseStartSprites[0];
        }
    }

    void UIUpdate()
    {
        if (isPlaying)
        {
            beatNowTxt.text = RecordConductor.instance.songPosInBeats.ToString("0.00");

            songPlaySlider.value = RecordConductor.instance.songAudioSource.time / RecordConductor.instance.songAudioSource.clip.length;
        }
    }

    private void OnPauseStartBtnClicked()
    {
        isPlaying = isPlaying ? false : true;
        if (isPlaying)
        {
            pauseStartBtn.image.sprite = pauseStartSprites[1];
            RecordConductor.instance.songAudioSource.Play();
        }
        else
        {
            pauseStartBtn.image.sprite = pauseStartSprites[0];
            RecordConductor.instance.songAudioSource.Pause();
        }
    }

    private void OnSliderValueChanged()
    {
        RecordConductor.instance.songAudioSource.time = RecordConductor.instance.songAudioSource.clip.length * songPlaySlider.value;

        RecordConductor.instance.songPosInBeats = RecordConductor.instance.songAudioSource.time / RecordConductor.instance.secPerBeat;

        beatNowTxt.text = RecordConductor.instance.songPosInBeats.ToString("0.00");
    }

}
