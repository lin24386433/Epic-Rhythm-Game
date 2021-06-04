using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    [SerializeField]
    public MainMenuDataController dataCtrl;

    [SerializeField]
    private OptionWindow optionWindow;

    [Space(20)]
    [SerializeField]
    private Text highScoreTxt;

    [SerializeField]
    private Text difficultyTxt;

    [SerializeField]
    private Image rankImg;

    [Space(20)]
    [SerializeField]
    private Sprite[] rankSprites;

    [SerializeField]
    private string[] difficultyHex;

    SongData data;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SetSongInfo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionWindow.gameObject.activeSelf)
        {
            optionWindow.gameObject.SetActive(true);
        }
    }

    public IEnumerator SetSongInfo()
    {
        data = dataCtrl.SongDataLoadedFromJson();

        yield return new WaitForSeconds(0.1f);

        highScoreTxt.text = data.playerHighScore.ToString();
        difficultyTxt.text = data.songDifficulty.ToString();     

        float acc;

        acc = ((float)data.playerHighScore / (float)data.maxScore) * 100;


        if (acc >= 96)
        {
            rankImg.sprite = rankSprites[0];
        }
        else if (acc >= 90)
        {
            rankImg.sprite = rankSprites[1];
        }
        else if (acc >= 80)
        {
            rankImg.sprite = rankSprites[2];
        }
        else if (acc >= 70)
        {
            rankImg.sprite = rankSprites[3];
        }
        else
        {
            rankImg.sprite = rankSprites[4];
        }

        Color color;

        switch (data.songDifficulty)
        {
            case SongDifficulty.Easy:
                if (ColorUtility.TryParseHtmlString(difficultyHex[0], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Normal:
                if (ColorUtility.TryParseHtmlString(difficultyHex[1], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Hard:
                if (ColorUtility.TryParseHtmlString(difficultyHex[2], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Expert:
                if (ColorUtility.TryParseHtmlString(difficultyHex[3], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Master:
                if (ColorUtility.TryParseHtmlString(difficultyHex[4], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
        }

    }


}
