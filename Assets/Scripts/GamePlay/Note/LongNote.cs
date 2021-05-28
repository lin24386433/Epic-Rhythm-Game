using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour
{
    public GameObject startNote;

    public GameObject middleNote;

    public GameObject endNote;

    private Conductor conductor;

    private Transform startPos;

    private Transform endPos;

    private float beat;

    public bool moving = true;

    public void Initialize(Conductor conductor, Transform startPoint, Transform endPoint)
    {
        this.conductor = conductor;
        this.startPos = startPoint;
        this.endPos = endPoint;
        
    }

    private void Update()
    {

        beat = (endNote.GetComponent<Note>().beat + startNote.GetComponent<Note>().beat) / 2f;
       
        middleNote.transform.localScale = new Vector2(startNote.transform.localScale.x, ((endNote.transform.localPosition.y - startNote.transform.localPosition.y) - 1) / 9.62f);

        if (moving)
        {
            middleNote.transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));
            startNote.GetComponent<Note>().moving = true;
        }
        else
        {
            middleNote.transform.localPosition = (startNote.transform.localPosition + endNote.transform.localPosition) / 2.0f;
            startNote.GetComponent<Note>().moving = false;
        }

        if(middleNote.transform.localScale.y <= -0.2f)
        {
            Destroy(this.gameObject);
        }

        //Debug.Log(Vector2.Distance(startNote.transform.localPosition, endNote.transform.localPosition));

        /*
        if(Vector2.Distance(startNote.transform.localPosition, endNote.transform.localPosition) <= 0f)
        {
            Destroy(this.gameObject);
        }
        */
    }
}
