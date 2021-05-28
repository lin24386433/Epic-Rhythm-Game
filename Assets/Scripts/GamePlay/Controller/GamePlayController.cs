using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;

    public GameObject mask;

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

        mask.GetComponent<Animator>().SetBool("Start", true);
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
        comboTxt.gameObject.transform.localScale = new Vector2(1.35f, 1.35f);
        StartCoroutine(ComboAnimate());
    }
    public void ResetCombo()
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
