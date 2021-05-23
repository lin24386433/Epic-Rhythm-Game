using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RecorderDataController : MonoBehaviour
{
    // All Recorders 
    [SerializeField]
    private CircleNoteRecorder[] circleRecorders;

    [SerializeField]
    private NoteRecorder[] noteRecorders;

    public string songName = "Gurenge";

    [SerializeField]
    private Button saveBtn;

    // Save Datas
    private NotesData notesDataToSave;

    private NotesData notesDataToLoad;

    private void Start()
    {
        saveBtn.onClick.AddListener(OnSaveBtnClicked);

        notesDataToLoad = NotesDataLoadedFromJson();

        SetLoadedDataToAllRecorder();

        
    }

    private void NotesDataSaveToJson(NotesData data)
    {
        // Path Setting : Application.dataPath/SongDatas/[songName]/NotesData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        // Directory : SongDatas
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Directory : [songName]
        path = Path.Combine(path, songName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // File : NotesData.txt
        path = Path.Combine(path, "NotesData" + ".txt");

        // Data To Json String
        string jsonInfo = JsonUtility.ToJson(data, true);

        // Json String Save in text file
        File.WriteAllText(path, jsonInfo);

        Debug.Log("寫入完成");
        Debug.Log("dataPath: " + path);
    }

    private NotesData NotesDataLoadedFromJson()
    {
        string loadData;

        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, songName);

        path = Path.Combine(path, "NotesData" + ".txt");

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<NotesData>(loadData);

    }

    float[] ListToArray(List<float> listToArray)
    {
        float[] array = new float[listToArray.Count];
        for(int i = 0; i < listToArray.Count; i++)
        {
            array[i] = listToArray[i];
        }
        return ArraySort(array);
    }

    private float[] ArraySort(float[] arrayIn)
    {
        float temp;

        for(int i = arrayIn.Length - 1; i > 0; i--)
        {
            for(int j = 0; j <= i - 1; j++)
            {
                if(arrayIn[j] > arrayIn[j + 1])
                {
                    temp = arrayIn[j];
                    arrayIn[j] = arrayIn[j + 1];
                    arrayIn[j + 1] = temp;
                }
            }
        }
        return arrayIn;
    }

    List<T> ArrayToList<T>(T[] arrayToList)
    {
        List<T> list = new List<T>();

        foreach(T t in arrayToList)
        {
            list.Add(t);
        }

        return list;
    }

    private void OnSaveBtnClicked()
    {
        notesDataToSave = new NotesData();

        #region data
        notesDataToSave.circleNote_Single1 = ListToArray(circleRecorders[0].singleNote);
        notesDataToSave.circleNote_Single2 = ListToArray(circleRecorders[1].singleNote);
        notesDataToSave.circleNote_Single3 = ListToArray(circleRecorders[2].singleNote);
        notesDataToSave.circleNote_Single4 = ListToArray(circleRecorders[3].singleNote);
        notesDataToSave.circleNote_Single5 = ListToArray(circleRecorders[4].singleNote);

        notesDataToSave.singleNote1 = ListToArray(noteRecorders[0].singleNote);
        notesDataToSave.longNoteStart1 = ListToArray(noteRecorders[0].longNoteStart);
        notesDataToSave.longNoteEnd1 = ListToArray(noteRecorders[0].longNoteEnd);

        notesDataToSave.singleNote2 = ListToArray(noteRecorders[1].singleNote);
        notesDataToSave.longNoteStart2 = ListToArray(noteRecorders[1].longNoteStart);
        notesDataToSave.longNoteEnd2 = ListToArray(noteRecorders[1].longNoteEnd);

        notesDataToSave.singleNote3 = ListToArray(noteRecorders[2].singleNote);
        notesDataToSave.longNoteStart3 = ListToArray(noteRecorders[2].longNoteStart);
        notesDataToSave.longNoteEnd3 = ListToArray(noteRecorders[2].longNoteEnd);

        notesDataToSave.singleNote4 = ListToArray(noteRecorders[3].singleNote);
        notesDataToSave.longNoteStart4 = ListToArray(noteRecorders[3].longNoteStart);
        notesDataToSave.longNoteEnd4 = ListToArray(noteRecorders[3].longNoteEnd);

        notesDataToSave.singleNote5 = ListToArray(noteRecorders[4].singleNote);
        notesDataToSave.longNoteStart5 = ListToArray(noteRecorders[4].longNoteStart);
        notesDataToSave.longNoteEnd5 = ListToArray(noteRecorders[4].longNoteEnd);
        #endregion

        NotesDataSaveToJson(notesDataToSave);
    }

    private void SetLoadedDataToAllRecorder()
    {
        circleRecorders[0].singleNote = ArrayToList(notesDataToLoad.circleNote_Single1);
        circleRecorders[1].singleNote = ArrayToList(notesDataToLoad.circleNote_Single2);
        circleRecorders[2].singleNote = ArrayToList(notesDataToLoad.circleNote_Single3);
        circleRecorders[3].singleNote = ArrayToList(notesDataToLoad.circleNote_Single4);
        circleRecorders[4].singleNote = ArrayToList(notesDataToLoad.circleNote_Single5);

        noteRecorders[0].singleNote = ArrayToList(notesDataToLoad.singleNote1);
        noteRecorders[0].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart1);
        noteRecorders[0].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd1);

        noteRecorders[1].singleNote = ArrayToList(notesDataToLoad.singleNote2);
        noteRecorders[1].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart2);
        noteRecorders[1].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd2);

        noteRecorders[2].singleNote = ArrayToList(notesDataToLoad.singleNote3);
        noteRecorders[2].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart3);
        noteRecorders[2].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd3);

        noteRecorders[3].singleNote = ArrayToList(notesDataToLoad.singleNote4);
        noteRecorders[3].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart4);
        noteRecorders[3].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd4);

        noteRecorders[4].singleNote = ArrayToList(notesDataToLoad.singleNote5);
        noteRecorders[4].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart5);
        noteRecorders[4].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd5);

    }

}
