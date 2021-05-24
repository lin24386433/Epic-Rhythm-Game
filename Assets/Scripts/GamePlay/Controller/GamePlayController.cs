using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;

    private int score = 0;

    private int combo = 0;

    // UI
    [SerializeField]
    private Text scoreTxt;

    [SerializeField]
    private Text comboTxt;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        scoreTxt.text = score.ToString();
        comboTxt.text = combo.ToString();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreTxt.text = score.ToString();
    }

    public void AddCombo()
    {
        combo += 1;
        comboTxt.text = combo.ToString();
    }
    public void ResetCombo()
    {
        combo = 0;
        comboTxt.text = "";
    }


}
