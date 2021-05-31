using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ConcludeController : MonoBehaviour
{
    [SerializeField]
    private Text comboTxt;

    [SerializeField]
    private Text scoreTxt;

    [SerializeField]
    private Text bestScoreTxt;

    [SerializeField]
    private Text perfectTxt;

    [SerializeField]
    private Text goodTxt;

    [SerializeField]
    private Text badTxt;

    [SerializeField]
    private Text missTxt;

    [Space(20)]
    [SerializeField]
    private Text songNameTxt;

    [SerializeField]
    private Text difficultyTxt;

    [Space(20)]
    [SerializeField]
    private Image rankPanel;

    [SerializeField]
    private Image newBestPanel;

    [Space(20)]
    [SerializeField]
    private Button nextBtn;

    [SerializeField]
    private Button retryBtn;

    [Space(20)]
    [SerializeField]
    private Sprite[] rankSprites;

    [SerializeField]
    private string[] difficultyHex;

    [Space(20)]
    [SerializeField]
    private float addNumberSpeed = 200f;

    [SerializeField]
    private AudioSource bgAudioSource;

    private AudioSource audioSource;

    SongData songData;

    float perfectCount = -1;
    float goodCount = -1;
    float badCount = -1;
    float missCount = -1;

    float combo = -1;
    float score = -1;
    bool isBestScore = false;

    private void Awake()
    {
        songData = SongDataLoadedFromJson();

        if (GameInfo.gameScore >= songData.playerHighScore)
        {
            songData.playerHighScore = GameInfo.gameScore;
            isBestScore = true;
        }
        if (GameInfo.gameCombo >= songData.playerHighCombo)
        {
            songData.playerHighCombo = GameInfo.gameCombo;
        }
        songData.playTimes++;

        SongDataSaveToJson(songData);

        songNameTxt.text = GameInfo.songName;

        Ranking();

        Color color;

        switch (songData.songDifficulty)
        {
            case SongDifficulty.Easy:
                difficultyTxt.text = "Easy";
                if (ColorUtility.TryParseHtmlString(difficultyHex[0], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Normal:
                difficultyTxt.text = "Normal";
                if (ColorUtility.TryParseHtmlString(difficultyHex[1], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Hard:
                difficultyTxt.text = "Hard";
                if (ColorUtility.TryParseHtmlString(difficultyHex[2], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Expert:
                difficultyTxt.text = "Expert";
                if (ColorUtility.TryParseHtmlString(difficultyHex[3], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
            case SongDifficulty.Master:
                difficultyTxt.text = "Master";
                if (ColorUtility.TryParseHtmlString(difficultyHex[4], out color))
                {
                    difficultyTxt.color = color;
                }
                break;
        }

        

        bestScoreTxt.text = "Best Score : " + songData.playerHighScore.ToString();


    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudio();

        nextBtn.onClick.AddListener(OnNextBtnClicked);
        retryBtn.onClick.AddListener(OnRetryBtnClicked);
    }

    private void PlayAudio()
    {
        scoreTxt.gameObject.GetComponent<AudioSource>().Play();
        bgAudioSource.Play();
    }

    private void Update()
    {
        TextAni();


    }

    public void Ranking()
    {
        float acc;

        acc = ((float)GameInfo.gameScore / (float)songData.maxScore) * 100;

        if (acc >= 96)
        {
            rankPanel.sprite = rankSprites[0];
        }
        else if (acc >= 90)
        {
            rankPanel.sprite = rankSprites[1];
        }
        else if (acc >= 80)
        {
            rankPanel.sprite = rankSprites[2];
        }
        else if (acc >= 70)
        {
            rankPanel.sprite = rankSprites[3];
        }
        else
        {
            rankPanel.sprite = rankSprites[4];
        }
    }

    private void SongDataSaveToJson(SongData data)
    {
        // Path Setting : Application.dataPath/SongDatas/[songName]/NotesData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        // Directory : SongDatas
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Directory : [songName]
        path = Path.Combine(path, GameInfo.songName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // File : NotesData.txt
        path = Path.Combine(path, "SongData" + ".txt");

        // Data To Json String
        string jsonInfo = JsonUtility.ToJson(data, true);

        // Json String Save in text file
        File.WriteAllText(path, jsonInfo);

        Debug.Log("寫入完成");
        Debug.Log("dataPath: " + path);
    }

    private SongData SongDataLoadedFromJson()
    {
        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, GameInfo.songName);

        path = Path.Combine(path, "SongData" + ".txt");

        string loadData;

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<SongData>(loadData);

    }

    private void TextAni()
    {
        if (score > GameInfo.gameScore)
        {
            score = GameInfo.gameScore;
            scoreTxt.text = ((int)score).ToString();
            scoreTxt.gameObject.GetComponent<AudioSource>().loop = false;
            StartCoroutine(WaitForPlayAnimation());
        }
        else if (score != GameInfo.gameScore)
        {
            scoreTxt.text = ((int)score).ToString();
            score += Time.deltaTime * addNumberSpeed * 500;
        }

        if (combo > GameInfo.gameCombo)
        {
            combo = GameInfo.gameCombo;
            comboTxt.text = ((int)combo).ToString() + "/" + songData.maxCombo;
        }
        else if (combo != GameInfo.gameCombo)
        {
            comboTxt.text = ((int)combo).ToString() + "/" + songData.maxCombo;
            combo += Time.deltaTime * addNumberSpeed;
        }

        if (perfectCount > GameInfo.perfectCount)
        {
            perfectCount = GameInfo.perfectCount;
            perfectTxt.text = ((int)perfectCount).ToString("000000");
        }
        else if (perfectCount != GameInfo.perfectCount)
        {
            perfectTxt.text = ((int)perfectCount).ToString("000000");
            perfectCount += Time.deltaTime * addNumberSpeed;
        }

        if (goodCount > GameInfo.goodCount)
        {
            goodCount = GameInfo.goodCount;
            goodTxt.text = ((int)goodCount).ToString("000000");
        }
        else if (goodCount != GameInfo.goodCount)
        {
            goodTxt.text = ((int)goodCount).ToString("000000");
            goodCount += Time.deltaTime * addNumberSpeed;
        }

        if (badCount > GameInfo.badCount)
        {
            badCount = GameInfo.badCount;
            badTxt.text = ((int)badCount).ToString("000000");
        }
        else if (badCount != GameInfo.badCount)
        {
            badTxt.text = ((int)badCount).ToString("000000");
            badCount += Time.deltaTime * addNumberSpeed;
        }

        if (missCount > GameInfo.missCount)
        {
            missCount = GameInfo.missCount;
            missTxt.text = ((int)missCount).ToString("000000");
        }
        else if (missCount != GameInfo.missCount)
        {
            missTxt.text = ((int)missCount).ToString("000000");
            missCount += Time.deltaTime * addNumberSpeed;
        }
    }

    private IEnumerator WaitForPlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        
        bestScoreTxt.gameObject.SetActive(true);
        if(isBestScore)
            newBestPanel.gameObject.SetActive(true);
        rankPanel.gameObject.SetActive(true);

        nextBtn.gameObject.SetActive(true);
        retryBtn.gameObject.SetActive(true);
    }

    private IEnumerator WaitForChangeScene(int sceneID)
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(sceneID);

    }

    private void OnNextBtnClicked()
    {
        nextBtn.gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(WaitForChangeScene(0));
    }

    private void OnRetryBtnClicked()
    {
        retryBtn.gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(WaitForChangeScene(1));
    }



}
