using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDetecter : MonoBehaviour
{
    [SerializeField]
    private AudioSource beatSound;

    [SerializeField]
    public KeyCode keyToPress;

    [SerializeField]
    private GameObject[] effects;

    //
    bool canDestroy = false;
    
    private GameObject obj;

    [SerializeField]
    private GameObject point;

    private void Start()
    {
        beatSound = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if(canDestroy)
        {
            if (Input.GetKeyDown(keyToPress) && !GamePlayController.instance.isPaused)
            {
                

                if (obj == null)
                    return;

                DetectHitAccuracy(obj);

                GameObject objToDelete = obj;
                obj = null;
                Destroy(objToDelete);
                
                beatSound.Play();
            }
            
        }
        if (Input.GetKeyDown(keyToPress) && !GamePlayController.instance.isPaused)
        {
            point.transform.localScale = new Vector2(1.2f, 1.2f);
        }
        if (Input.GetKeyUp(keyToPress) && !GamePlayController.instance.isPaused)
        {
            point.transform.localScale = new Vector2(1, 1);
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
                GamePlayController.instance.AddScore(ScoreType.Miss);
            }
            obj = null;
        }
           
    }
}
