using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class OptionWindow : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private Button helpBtn;

    [Space(20)]     // -------------------------------------------------------------------------------------

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Text volumeTxt;

    [Space(20)]     // -------------------------------------------------------------------------------------
    [System.NonSerialized]
    int speedStatus = 1;

    [SerializeField]
    private string[] speedColors;

    [SerializeField]
    private Button[] speed_Btns;

    [Space(20)]     // -------------------------------------------------------------------------------------

    bool canCustomKey = false;

    int customBtnIndex = 0;

    [SerializeField]
    private Button[] customize_Btns;

    public KeyCode[] keyCodes;

    PlayerSetting playerSetting;

    // Start is called before the first frame update
    void Start()
    {
        LoadedDataSetting();

        UISetting();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canCustomKey)
        {
            canCustomKey = false;

            customize_Btns[customBtnIndex].GetComponentInChildren<Text>().text = keyCodes[customBtnIndex].ToString();

            UpdatePlayerSetting();
        }

    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && canCustomKey)
        {
            keyCodes[customBtnIndex] = e.keyCode;

            customize_Btns[customBtnIndex].GetComponentInChildren<Text>().text = keyCodes[customBtnIndex].ToString();

            canCustomKey = false;

            UpdatePlayerSetting();
        }

    }

    void UISetting()
    {
        volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });

        speed_Btns[0].onClick.AddListener(delegate { OnSpeedBtnClicked(0); });
        speed_Btns[1].onClick.AddListener(delegate { OnSpeedBtnClicked(1); });
        speed_Btns[2].onClick.AddListener(delegate { OnSpeedBtnClicked(2); });

        for (int i = 0; i < customize_Btns.Length; i++)
        {
            int copy = i;

            customize_Btns[i].onClick.AddListener(delegate { OnCustomizeBtnsClicked(copy); });

            customize_Btns[i].GetComponentInChildren<Text>().text = keyCodes[i].ToString();
        }

        exitBtn.onClick.AddListener(delegate { UpdatePlayerSetting(); this.gameObject.SetActive(false);  });
    }

    void LoadedDataSetting()
    {
        playerSetting = MainMenuController.instance.dataCtrl.PlayerSettingLoadedFromJson();

        volumeSlider.value = playerSetting.volume;

        volumeTxt.text = playerSetting.volume.ToString();

        float value = playerSetting.volume / 2f - 50f;

        audioMixer.SetFloat("MasterVolume", value);

        speedStatus = playerSetting.noteSpeed;

        Color color;

        foreach (Button btn in speed_Btns)
        {
            if (ColorUtility.TryParseHtmlString("#999999", out color))
                btn.GetComponentInChildren<Text>().color = color;
        }

        if (ColorUtility.TryParseHtmlString(speedColors[speedStatus], out color))
            speed_Btns[speedStatus].GetComponentInChildren<Text>().color = color;

        for (int i = 0; i < keyCodes.Length; i++)
        {
            keyCodes[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), playerSetting.keyCodes[i]);
        }
    }

    private void OnSliderValueChanged()
    {
        volumeTxt.text = volumeSlider.value.ToString();

        float value = volumeSlider.value / 2f - 50f;

        audioMixer.SetFloat("MasterVolume", value);

        UpdatePlayerSetting();
    }

    private void OnSpeedBtnClicked(int index)
    {
        speedStatus = index;

        Color color;

        foreach (Button btn in speed_Btns)
        {
            if (ColorUtility.TryParseHtmlString("#999999", out color))
                btn.GetComponentInChildren<Text>().color = color;
        }

        if (ColorUtility.TryParseHtmlString(speedColors[index], out color))
            speed_Btns[index].GetComponentInChildren<Text>().color = color;

        UpdatePlayerSetting();

    }

    private void OnCustomizeBtnsClicked(int index)
    {
        canCustomKey = true;

        customBtnIndex = index;

        customize_Btns[index].GetComponentInChildren<Text>().text = "";

    }

    private void UpdatePlayerSetting()
    {
        playerSetting.volume = (int)volumeSlider.value;
        playerSetting.noteSpeed = speedStatus;
        for(int i = 0; i < keyCodes.Length; i++)
        {
            playerSetting.keyCodes[i] = keyCodes[i].ToString();
        }
        PlayerSaveToJson(playerSetting);
    }

    private void PlayerSaveToJson(PlayerSetting dataToSave)
    {
        
        string path = Path.Combine(Application.dataPath, "Player.txt");

        if (!File.Exists(path))
        {
            PlayerSetting data = new PlayerSetting();

            // Data To Json String
            string jsonInfo1 = JsonUtility.ToJson(data, true);

            // Json String Save in text file
            File.WriteAllText(path, jsonInfo1);

            return;
        }

        // Data To Json String
        string jsonInfo = JsonUtility.ToJson(dataToSave, true);

        // Json String Save in text file
        File.WriteAllText(path, jsonInfo);

    }

}
