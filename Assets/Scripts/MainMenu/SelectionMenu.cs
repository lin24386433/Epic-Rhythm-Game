using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenu : MonoBehaviour
{
    private float angle = 0f;

    public MainMenuDataController dataController;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private RectTransform[] menuBars;

    [SerializeField]
    private Text[] menuBarTxts;

    [SerializeField]
    private GameObject info;

    [SerializeField]
    private AudioSource musicPlayer;

    [SerializeField]
    private Image backgroundImg;

    [SerializeField]
    private Sprite[] unselectedPoints;

    [SerializeField]
    private Sprite selectedPoint;

    private int index = 0;

    private int selectedIndex = 0;

    private AudioSource audioSource;

    private Animator animator;

    // selecting var
    float timer = 0f;
    bool onSelecting = false;
    float stopTime = 0.5f;

    // enter play scene
    bool isGoingToPlay = false;
    public GameObject mask;

    private void Start()
    {
        SelectionMenuUpdate();

        musicPlayer.PlayOneShot(dataController.audioClips[selectedIndex]);

        Texture2D myImg = dataController.backgroundImages[selectedIndex];

        Sprite sprite = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), Vector2.zero);

        backgroundImg.sprite = sprite;

        audioSource = GetComponent<AudioSource>();

        animator = info.GetComponent<Animator>();
    }

    private void Update()
    {
        if (onSelecting)
        {
            timer += Time.deltaTime;
            if(timer >= stopTime)   // selecting end
            {
                onSelecting = false;
                timer = 0f;

                animator.SetBool("onSelecting", false);

                Texture2D myImg = dataController.backgroundImages[selectedIndex];
                Sprite sprite = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), Vector2.zero);
                backgroundImg.sprite = sprite;
                
                musicPlayer.Stop();
                musicPlayer.PlayOneShot(dataController.audioClips[selectedIndex]);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            angle -= 72;
            index = (int)((int)angle / 72);

            onSelecting = true;
            timer = 0f;

            animator.SetBool("onSelecting", true);

            SelectionMenuUpdate();

            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            angle += 72;
            index = (int)((int)angle / 72);

            onSelecting = true;
            timer = 0f;

            animator.SetBool("onSelecting", true);

            SelectionMenuUpdate();

            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Return) && !isGoingToPlay)
        {
            StartCoroutine(GamePlay());
            isGoingToPlay = true;
        }

        if (isGoingToPlay)
        {
            musicPlayer.volume = Mathf.Lerp(musicPlayer.volume, 0f, Time.deltaTime * 1f);
        }

    }

    private IEnumerator GamePlay()
    {
        this.GetComponent<Animator>().SetBool("isExited", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("onSelecting", true);       

        yield return new WaitForSeconds(1f);
        mask.GetComponent<Animator>().SetBool("Start", false);
       

        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(1);
    }

    public void SelectionMenuUpdate()
    {
        int songNameCount = dataController.songNames.Count;

        int barNow = index >= 0 ? index % menuBars.Length : (index % menuBars.Length == 0) ? 0 : menuBars.Length - Mathf.Abs(index % menuBars.Length);

        if (index >= 0)
        {
            menuBarTxts[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].text = dataController.songNames[(index % songNameCount - 1) < 0 ? songNameCount - 1 : index % songNameCount - 1];
            menuBars[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].localScale = new Vector2(0.9f,0.9f);
            menuBars[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].GetComponentsInChildren<Image>()[1].sprite = unselectedPoints[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1];

            menuBarTxts[barNow].text = dataController.songNames[index % songNameCount];
            menuBars[barNow].localScale = new Vector2(1.2f, 1.2f);
            menuBars[barNow].GetComponentsInChildren<Image>()[1].sprite = selectedPoint;

            GameInfo.songName = dataController.songNames[index % songNameCount];

            menuBarTxts[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].text = dataController.songNames[(index % songNameCount + 1) >= songNameCount ? 0 : index % songNameCount + 1];
            menuBars[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].localScale = new Vector2(0.9f, 0.9f);
            menuBars[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].GetComponentsInChildren<Image>()[1].sprite = unselectedPoints[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1];

            selectedIndex = index % songNameCount;

        }
        else
        {
            int x = (index % songNameCount == 0) ? 0 : songNameCount - Mathf.Abs(index % songNameCount);          

            menuBarTxts[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].text = dataController.songNames[songNameCount - Mathf.Abs(index % songNameCount) - 1];
            menuBars[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].localScale = new Vector2(0.9f, 0.9f);
            menuBars[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1].GetComponentsInChildren<Image>()[1].sprite = unselectedPoints[(barNow - 1) < 0 ? menuBars.Length - 1 : barNow - 1];

            menuBarTxts[barNow].text = dataController.songNames[x];
            menuBars[barNow].localScale = new Vector2(1.2f, 1.2f);
            menuBars[barNow].GetComponentsInChildren<Image>()[1].sprite = selectedPoint;

            GameInfo.songName = dataController.songNames[x];

            menuBarTxts[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].text = (x + 1) >= songNameCount ? dataController.songNames[0] : dataController.songNames[x + 1];
            menuBars[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].localScale = new Vector2(0.9f, 0.9f);
            menuBars[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1].GetComponentsInChildren<Image>()[1].sprite = unselectedPoints[(barNow + 1) >= menuBars.Length ? 0 : barNow + 1];

            selectedIndex = x;

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
