using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;

    public GamePlayTransitionController transitionController;

    public GameObject mask;

    [System.NonSerialized]
    public bool isPaused = false;

    public float perfectOffset = 0.3f;

    public float goodOffset = 0.5f;

    private int score = 0;

    private int combo = 0;

    private int highCombo = 0;

    private int perfectCount = 0;

    private int goodCount = 0;

    private int badCount = 0;

    private int missCount = 0;

    // UI
    [SerializeField]
    private Text scoreTxt;

    [SerializeField]
    private Text comboTxt;

    [SerializeField]
    private Text songNameTxt;

    [System.NonSerialized]
    public KeyCode[] keyCodes = new KeyCode[5];

    public RazerLine[] razerlines;

    public ShowLineController[] showlines;

    public CircleDetecter[] circleDetecters;

    private void Awake()
    {
        if (instance == null)
            instance = this;


        scoreTxt.text = score.ToString();
        
        songNameTxt.text = GameInfo.songName;

        //mask.GetComponent<Animator>().SetBool("Start", true);
    }

    private void Start()
    {
        for(int i = 0; i < keyCodes.Length; i++)
        {
            circleDetecters[i].keyToPress = keyCodes[i];
            razerlines[i].keyToPress = keyCodes[i];
            showlines[i].keyToPress = keyCodes[i];

            razerlines[i].keyToPress2 = keyCodes[i+1 >= keyCodes.Length? 0 : i+1];
            showlines[i].keyToPress2 = keyCodes[i + 1 >= keyCodes.Length ? 0 : i + 1];
        }
        
    }

    public void SongFinished()
    {
        GameInfo.perfectCount = perfectCount;
        GameInfo.goodCount = goodCount;
        GameInfo.badCount = badCount;
        GameInfo.missCount = missCount;
        GameInfo.gameScore = score;
        GameInfo.gameCombo = highCombo;

        StartCoroutine(transitionController.ExitScene(3));

        

    }

    public void AddScore(ScoreType type)
    {
        score += (int)type;
        scoreTxt.text = score.ToString();
        switch (type)
        {
            case ScoreType.Perfect:
                AddCombo();
                perfectCount++;
                break;
            case ScoreType.Good:
                AddCombo();
                goodCount++;
                break;
            case ScoreType.Bad:
                ResetCombo();
                badCount++;
                break;
            case ScoreType.Miss:
                ResetCombo();
                missCount++;
                break;
        }
    }

    private void AddCombo()
    {
        combo += 1;

        if(combo >= highCombo)
        {
            highCombo = combo;
        }

        comboTxt.text = combo.ToString();
        comboTxt.gameObject.transform.localScale = new Vector2(1.35f, 1.35f);
        StartCoroutine(ComboAnimate());
    }

    private void ResetCombo()
    {
        combo = 0;
        comboTxt.text = "";
    }

    private IEnumerator ComboAnimate()
    {
        yield return new WaitForSeconds(0.08f);
        comboTxt.gameObject.transform.localScale = new Vector2(1, 1);
    }

}
