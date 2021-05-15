using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderLongNote : MonoBehaviour
{
    public GameObject startNote;

    public GameObject middleNote;

    public GameObject endNote;

    private RecordConductor conductor;

    private Transform startPos;

    private Transform endPos;

    private float beat;

    public bool moving = true;

    public void Initialize(RecordConductor conductor, Transform startPoint, Transform endPoint)
    {
        this.conductor = conductor;
        this.startPos = startPoint;
        this.endPos = endPoint;

    }

    private void Update()
    {

        beat = (endNote.GetComponent<RecorderNote>().beat + startNote.GetComponent<RecorderNote>().beat) / 2f;

        middleNote.transform.localScale = new Vector2(startNote.transform.localScale.x, ((endNote.transform.localPosition.y - startNote.transform.localPosition.y) - 1) / 9.656f);

        if (moving)
        {
            middleNote.transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));
            startNote.GetComponent<RecorderNote>().moving = true;
        }
        else
        {
            middleNote.transform.localPosition = (startNote.transform.localPosition + endNote.transform.localPosition) / 2.0f;
            startNote.GetComponent<RecorderNote>().moving = false;
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
