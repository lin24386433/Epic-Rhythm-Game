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

                DetectHitAccuracy(obj);

                GameObject objToDelete = obj;
                obj = null;
                Destroy(objToDelete);
                
                beatSound.Play();
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
