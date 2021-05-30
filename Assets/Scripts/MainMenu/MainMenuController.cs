using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    [SerializeField]
    private MainMenuDataController dataCtrl;

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
        SetSongInfo();
    }

    public void SetSongInfo()
    {
        data = dataCtrl.SongDataLoadedFromJson();
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
    }

}
