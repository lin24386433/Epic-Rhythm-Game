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
    private GameObject[] effects;

    //
    private bool canDestroy = false;

    public GameObject obj;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canDestroy)
        {
            // Down
            if ((Input.GetKeyDown(keyToPress) && Input.GetKey(keyToPress2)) || (Input.GetKey(keyToPress) && Input.GetKeyDown(keyToPress2)))
            {
                if (obj == null)
                    return;

                if (obj.CompareTag("Note"))
                {
                    DetectHitAccuracy(obj);

                    GameObject objToDelete = obj;
                    obj = null;
                    Destroy(objToDelete);

                    beatSound.Play();
                }
                else if (obj.CompareTag("LongNoteStart"))
                {
                    if (obj.transform.GetComponentInParent<LongNote>().moving)
                    {
                        DetectHitAccuracy(obj);

                        obj.transform.GetComponentInParent<LongNote>().moving = false;

                        obj.transform.GetComponentInParent<LongNote>().isHolding = true;

                        obj.transform.GetComponentInParent<LongNote>().startNote.transform.position = this.transform.position;
                        beatSound.Play();
                    }
                }
            }

            // Up
            if ((Input.GetKeyUp(keyToPress) && Input.GetKey(keyToPress2)) || (Input.GetKey(keyToPress) && Input.GetKeyUp(keyToPress2)) || (Input.GetKeyUp(keyToPress) && Input.GetKeyUp(keyToPress2)))
            {
                if (obj == null)
                    return;
                if (obj.CompareTag("LongNoteEnd") && obj.transform.GetComponentInParent<LongNote>().isHolding)
                {
                    DetectHitAccuracy(obj);

                    GameObject objToDelete = obj.transform.parent.gameObject;
                    obj = null;
                    Destroy(objToDelete);

                    beatSound.Play();
                }
                else        // Miss
                {
                    if (obj.transform.parent != null)
                    {
                        if (obj.transform.GetComponentInParent<LongNote>().isHolding)
                        {
                            Destroy(obj.transform.parent.gameObject);
                            Instantiate(effects[3], this.transform.position, this.transform.rotation);
                            GamePlayController.instance.AddScore(ScoreType.Miss);
                        }
                        
                    }
                        
                }
            }

        }   

    }

    private void DetectHitAccuracy(GameObject beatIn)
    {
        float beat = beatIn.GetComponent<Note>().beat;

        float beatOffset = Mathf.Abs(beat - Conductor.instance.songPosInBeats);

        if (beatOffset <= GamePlayController.instance.perfectOffset)
        {
            Instantiate(effects[0], this.transform.position, this.transform.rotation);
            GamePlayController.instance.AddScore(ScoreType.Perfect);
        }
        else if (beatOffset <= GamePlayController.instance.goodOffset)
        {
            Instantiate(effects[1], this.transform.position, this.transform.rotation);
            GamePlayController.instance.AddScore(ScoreType.Good);
        }
        else
        {
            Instantiate(effects[2], this.transform.position, this.transform.rotation);
            GamePlayController.instance.AddScore(ScoreType.Bad);
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
            {
                canDestroy = true;
                obj = collision.gameObject;
            }
                
        }
        if(collision.CompareTag("LongNoteStart"))
        {
            if (obj == null)
            {
                canDestroy = true;
                obj = collision.gameObject;
            }
        }
        if(collision.CompareTag("LongNoteEnd"))
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
            if (obj != null)
            {
                Instantiate(effects[3], this.transform.position, this.transform.rotation);
                GamePlayController.instance.AddScore(ScoreType.Miss);
            }
            obj = null;
        }
        if (collision.CompareTag("LongNoteStart"))
        {
            canDestroy = false;
            obj = null;
            if (!collision.transform.GetComponentInParent<LongNote>().isHolding)
                GamePlayController.instance.AddScore(ScoreType.Miss);
        }
        if (collision.CompareTag("LongNoteEnd") && !collision.transform.GetComponentInParent<LongNote>().isHolding)
        {
            canDestroy = false;
            if (obj != null)
            {
                Instantiate(effects[3], this.transform.position, this.transform.rotation);
                GamePlayController.instance.AddScore(ScoreType.Miss);
            }
            obj = null;
        }
    }

}
