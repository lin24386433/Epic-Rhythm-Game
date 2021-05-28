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

    [SerializeField]
    private Dropdown dropDown;

    public string songSelected;

    private int selectedIndex = 0;


    public GameObject noteDetailWindow;

    [SerializeField]
    private Sprite[] pauseStartSprites;

    private void Start()
    {
        songSelected = GetComponent<RecorderDataController>().songsInFolder[0];

        pauseStartBtn.onClick.AddListener(OnPauseStartBtnClicked);

        songPlaySlider.onValueChanged.AddListener((delegate { OnSliderValueChanged(); }));

        dropDown.ClearOptions();

        dropDown.AddOptions(GetComponent<RecorderDataController>().songsInFolder);

        dropDown.onValueChanged.AddListener(delegate { OnDropDownValueChanged(); });

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

    private void OnDropDownValueChanged()
    {
        selectedIndex = dropDown.value;

        RecorderDataController dataController = GetComponent<RecorderDataController>();

        songSelected = dataController.songsInFolder[selectedIndex];

        beatNowTxt.text = 0.ToString("0.00");
        RecordConductor.instance.songAudioSource.time = 0f;
        StartCoroutine(dataController.SetAudioFromFileToConductor(songSelected));

        dataController.notesDataToLoad = dataController.NotesDataLoadedFromJson(songSelected);
        dataController.SongDataLoadedFromJson(songSelected);

        dataController.SetLoadedDataToAllRecorder();

        dataController.UpdateAllRecorder();

    }
}
