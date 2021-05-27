using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public float angle = 0f;

    public float speed = 1f;

    public RectTransform[] menuBars;

    public Text[] menuBarTxts;

    public GameObject info;

    public AudioSource audioPlayer;

    public string[] datas;

    public int index = 0;

    private AudioSource audioSource;

    private Animator animator;

    private void Awake()
    {
        SelectionMenuUpdate();
    }

    private void Start()
    {
        

        audioSource = GetComponent<AudioSource>();

        animator = info.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            angle -= 72;
            index = (int)((int)angle / 72);
            audioSource.Play();
            animator.SetTrigger("changing");
            audioPlayer.Play();
            SelectionMenuUpdate();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            angle += 72;
            index = (int)((int)angle / 72);
            audioSource.Play();
            animator.SetTrigger("changing");
            audioPlayer.Play();
            SelectionMenuUpdate();
        }

        
    }

    private void SelectionMenuUpdate()
    {
        int barNow = index >= 0 ? index % menuBars.Length : (index % menuBars.Length == 0) ? 0 : menuBars.Length - Mathf.Abs(index % menuBars.Length);

        if (index >= 0)
        {
            menuBarTxts[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].text = datas[(index % datas.Length - 1) < 0 ? datas.Length - 1 : index % datas.Length - 1];

            menuBarTxts[barNow].text = datas[index % datas.Length];

            menuBarTxts[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].text = datas[(index % datas.Length + 1) >= datas.Length ? 0 : index % datas.Length + 1];
        }
        else
        {
            int x = (index % datas.Length == 0) ? 0 : datas.Length - Mathf.Abs(index % datas.Length);

            menuBarTxts[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].text = datas[datas.Length - Mathf.Abs(index % datas.Length) - 1];

            menuBarTxts[barNow].text = datas[x];

            menuBarTxts[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].text = (x + 1) >= datas.Length ? datas[0] : datas[x + 1];
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), speed);

        foreach (RectTransform obj in menuBars)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            if(obj.transform.position.x <= this.transform.position.x)
            {
                obj.gameObject.SetActive(false);
            }
            else
            {
                obj.gameObject.SetActive(true);
            }
        }
    }
}
