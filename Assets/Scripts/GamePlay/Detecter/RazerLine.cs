using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerLine : MonoBehaviour
{ 
    private AudioSource beatSound;

    [Space(10)]
    [SerializeField]
    private KeyCode keyToPress;

    [SerializeField]
    private KeyCode keyToPress2;

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
    private bool canDestroy = false;

    private GameObject obj;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canDestroy)
        {
            if (Input.GetKeyDown(keyToPress) && Input.GetKey(keyToPress2) || Input.GetKey(keyToPress) && Input.GetKeyDown(keyToPress2))
            {
                if (obj == null)
                    return;

                float dis = Vector2.Distance(obj.transform.position, this.transform.position);

                if(dis <= perfectRange)
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

                if (obj.CompareTag("Note"))
                {
                    GameObject objToDelete = obj;
                    obj = null;
                    Destroy(objToDelete);

                    beatSound.Play();
                }
                else if (obj.CompareTag("LongNoteStart"))
                {
                    if (obj.transform.GetComponentInParent<LongNote>().moving)
                    {
                        obj.transform.GetComponentInParent<LongNote>().moving = false;

                        obj.transform.GetComponentInParent<LongNote>().startNote.transform.position = this.transform.position;
                        beatSound.Play();
                    }
                }
            }

            if (Input.GetKeyUp(keyToPress) && Input.GetKey(keyToPress2) || Input.GetKey(keyToPress) && Input.GetKeyUp(keyToPress2) || Input.GetKeyUp(keyToPress) && Input.GetKeyUp(keyToPress2))
            {
                if (obj == null)
                    return;
                if (obj.CompareTag("LongNoteEnd"))
                {
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

                    GameObject objToDelete = obj.transform.parent.gameObject;
                    obj = null;
                    Destroy(objToDelete);

                    beatSound.Play();
                }
                else        // Miss
                {
                    if (obj.transform.parent != null)
                    {
                        Destroy(obj.transform.parent.gameObject);
                        Instantiate(effects[3], this.transform.position, this.transform.rotation);
                        GamePlayController.instance.ResetCombo();
                    }
                        
                }
            }

        }   

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            canDestroy = true;
        }
        if (collision.CompareTag("LongNoteStart"))
        {
            canDestroy = true;
        }
        if (collision.CompareTag("LongNoteEnd"))
        {
            canDestroy = true;
            obj = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if(obj == null)
                obj = collision.gameObject;
        }
        if(collision.CompareTag("LongNoteStart"))
        {
            if (obj == null)
                obj = collision.gameObject;
        }
        if(collision.CompareTag("LongNoteEnd"))
        {
            if (obj == null)
                obj = collision.gameObject;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            canDestroy = false;
            if (obj != null)
            {
                Instantiate(effects[3], this.transform.position, this.transform.rotation);
                GamePlayController.instance.ResetCombo();
            }
            obj = null;
        }
        if (collision.CompareTag("LongNoteStart"))
        {
            canDestroy = false;
            obj = null;
        }
        if (collision.CompareTag("LongNoteEnd"))
        {
            canDestroy = false;
            if (obj != null)
            {
                Instantiate(effects[3], this.transform.position, this.transform.rotation);
                GamePlayController.instance.ResetCombo();
            }
            obj = null;
        }
    }

}
