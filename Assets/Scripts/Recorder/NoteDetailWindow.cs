using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteDetailWindow : MonoBehaviour
{
    private RecorderNote note;

    private RecorderLongNote longNote;

    private RecorderCircleNote circleNote;

    [SerializeField]
    private Toggle isLongNoteToggle;

    [SerializeField]
    private Text beatTxt1;

    [SerializeField]
    private Text beatTxt2;

    [SerializeField]
    private InputField inputField1;

    [SerializeField]
    private InputField inputField2;

    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private Button confirmBtn;

    public void Init(RecorderNote input)
    {
        note = input;

        isLongNoteToggle.gameObject.SetActive(true);

        isLongNoteToggle.isOn = false;

        beatTxt2.gameObject.SetActive(false);

        inputField2.gameObject.SetActive(false);

        beatTxt1.text = "Beat : ";

        inputField1.text = input.beat.ToString();


    }

    public void Init(RecorderLongNote input)
    {
        longNote = input;

        isLongNoteToggle.gameObject.SetActive(true);

        isLongNoteToggle.isOn = true;

        beatTxt2.gameObject.SetActive(true);

        inputField2.gameObject.SetActive(true);

        beatTxt1.text = "Start Beat : ";

        inputField1.text = input.startNote.GetComponent<RecorderNote>().beat.ToString();

        beatTxt2.text = "End Beat : ";

        inputField2.text = input.endNote.GetComponent<RecorderNote>().beat.ToString();
    }

    public void Init(RecorderCircleNote input)
    {
        circleNote = input;

        isLongNoteToggle.gameObject.SetActive(false);

        beatTxt2.gameObject.SetActive(false);

        inputField2.gameObject.SetActive(false);

        beatTxt1.text = "Beat : ";

        inputField1.text = input.beat.ToString();
    }

    private void Start()
    {
        isLongNoteToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(); });

        exitBtn.onClick.AddListener(OnExitBtnClicked);

        confirmBtn.onClick.AddListener(OnConfirmBtnClicked);

        beatTxt2.text = "End Beat : ";
    }

    private void OnToggleValueChanged()
    {
        if (isLongNoteToggle.isOn)
        {
            beatTxt2.gameObject.SetActive(true);

            inputField2.gameObject.SetActive(true);

            beatTxt1.text = "Start Beat : ";

        }
        else
        {
            beatTxt2.gameObject.SetActive(false);

            inputField2.gameObject.SetActive(false);

            beatTxt1.text = "Beat : ";
        }
    }
    
    private void OnExitBtnClicked()
    {
        note = null;
        longNote = null;
        circleNote = null;

        this.gameObject.SetActive(false);
    }

    private void OnConfirmBtnClicked()
    {
        if (isLongNoteToggle.isOn)
        {
            float startBeatToSave = float.Parse(inputField1.text);

            float endBeatToSave = float.Parse(inputField2.text);

            if (note != null)
            {
                note.recorder.DeleteNote(note.beat);

                note.recorder.longNoteStart.Add(startBeatToSave);

                note.recorder.longNoteEnd.Add(endBeatToSave);

                note.recorder.UpdateNote();

            }
            else if (longNote != null)
            {
                longNote.recorder.DeleteLongNote(longNote.startNote.GetComponent<RecorderNote>().beat, longNote.endNote.GetComponent<RecorderNote>().beat);

                longNote.recorder.longNoteStart.Add(startBeatToSave);

                longNote.recorder.longNoteEnd.Add(endBeatToSave);

                longNote.recorder.UpdateNote();
            }   
        }
        else
        {
            float beatToSave = float.Parse(inputField1.text);


            if (note != null)
            {
                note.recorder.DeleteNote(note.beat);

                note.recorder.singleNote.Add(beatToSave);

                note.recorder.UpdateNote();

            }
            else if (longNote != null)
            {
                longNote.recorder.DeleteLongNote(longNote.startNote.GetComponent<RecorderNote>().beat, longNote.endNote.GetComponent<RecorderNote>().beat);

                longNote.recorder.singleNote.Add(beatToSave);

                longNote.recorder.UpdateNote();
            }
            else if (circleNote != null)
            {
                circleNote.recorder.DeleteNote(circleNote.beat);

                circleNote.recorder.singleNote.Add(beatToSave);

                circleNote.recorder.UpdateNote();
            }
        }

        


        note = null;
        longNote = null;
        circleNote = null;

        this.gameObject.SetActive(false);
    }

}
