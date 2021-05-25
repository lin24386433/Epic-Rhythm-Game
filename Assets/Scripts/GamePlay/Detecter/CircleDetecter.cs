using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetecter : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    [SerializeField]
    private KeyCode keyToPress;

    [SerializeField]
    private float perfectRange = 0.5f;

    [SerializeField]
    private float goodRange = 0.7f;

    [SerializeField]
    private int perfectScore = 500;

    [SerializeField]
    private int goodScore = 300;

    [SerializeField]
    private int badScore = 100;

    [SerializeField]
    private GameObject[] effects;

    //
    bool canDestroy = false;

    [SerializeField]
    private GameObject obj;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(canDestroy)
        {
            if (Input.GetKeyDown(keyToPress))
            {
                if (obj == null)
                    return;

                float dis = Vector2.Distance(obj.transform.position, this.transform.position);

                if (dis <= perfectRange)
                {
                    Instantiate(effects[0], this.transform.position, this.transform.rotation);
                    GamePlayController.instance.AddCombo();
                    GamePlayController.instance.AddScore(perfectScore);
                }
                else if (dis <= goodRange)
                {
                    Instantiate(effects[1], this.transform.position, this.transform.rotation);
                    GamePlayController.instance.AddCombo();
                    GamePlayController.instance.AddScore(goodScore);
                }
                else
                {
                    Instantiate(effects[2], this.transform.position, this.transform.rotation);
                    GamePlayController.instance.AddCombo();
                    GamePlayController.instance.AddScore(badScore);
                }

                GameObject objToDelete = obj;
                obj = null;
                Destroy(objToDelete);
                
                beatSound.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
            canDestroy = true;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (obj == null)
            {
                canDestroy = true;
                obj = collision.gameObject;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note")) 
        {
            canDestroy = false;
            if (obj != null)    // Miss 
            {
                Instantiate(effects[3], this.transform.position, this.transform.rotation);
                GamePlayController.instance.ResetCombo();
            }
            obj = null;
        }
           
    }
}
