using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderLongNote : MonoBehaviour
{
    private RecordConductor conductor;

    [System.NonSerialized]
    public NoteRecorder recorder;

    private Transform startPos;

    private Transform endPos;

    private float beat;


    public GameObject startNote;

    public GameObject middleNote;

    public GameObject endNote;

    public bool moving = true;

    public void Initialize(RecordConductor conductor, NoteRecorder recorder, Transform startPoint, Transform endPoint)
    {
        this.conductor = conductor;
        this.recorder = recorder;
        this.startPos = startPoint;
        this.endPos = endPoint;

    }

    private void Update()
    {
        MouseInput(); 

        Movement();
    }

    #region FUNC:Movement
    private void Movement()
    {
        beat = (endNote.GetComponent<RecorderNote>().beat + startNote.GetComponent<RecorderNote>().beat) / 2f;

        middleNote.transform.localScale = new Vector2(startNote.transform.localScale.x, ((endNote.transform.localPosition.y - startNote.transform.localPosition.y) - 1) / 9.64f);

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
    }
    #endregion

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit == false)
                return;

            if (hit.collider.gameObject == this.middleNote && !GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.activeSelf && mousePos.y >= -2)
            {
                OnClicked(); 
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit == false)
                return;

            if (hit.collider.gameObject == this.middleNote && mousePos.y >= -2)
            {
                recorder.DeleteLongNote(startNote.GetComponent<RecorderNote>().beat, endNote.GetComponent<RecorderNote>().beat);
            }

        }
    }

    private void OnClicked()
    {
        GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.SetActive(true);
        GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.GetComponent<NoteDetailWindow>().Init(this);
    }
}
