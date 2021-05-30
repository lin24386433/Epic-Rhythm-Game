using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class RecorderDataController : MonoBehaviour
{
    // All Recorders 
    [SerializeField]
    private CircleNoteRecorder[] circleRecorders;

    [SerializeField]
    private NoteRecorder[] noteRecorders;

    public List<string> songsInFolder;

    [SerializeField]
    private Button saveBtn;

    // Save Datas
    private NotesData notesDataToSave;

    [System.NonSerialized]
    public NotesData notesDataToLoad;

    [SerializeField]
    private SongData recorderSongData;


    private void Awake()
    {
        songsInFolder = GetAllSongsFromFolder();


        notesDataToLoad = NotesDataLoadedFromJson(songsInFolder[0]);
        recorderSongData = SongDataLoadedFromJson(songsInFolder[0]);

        StartCoroutine(SetAudioFromFileToConductor(songsInFolder[0]));

        SetLoadedDataToAllRecorder();       
    }

    private void Start()
    {
        saveBtn.onClick.AddListener(OnSaveBtnClicked);
    }

    private List<string> GetAllSongsFromFolder()
    {
        List<string> list = new List<string>();

        string path = Path.Combine(Application.dataPath, "SongDatas");

        DirectoryInfo info = new DirectoryInfo(path);

        DirectoryInfo[] folders = info.GetDirectories();

        foreach (DirectoryInfo folder in folders)
        {
            list.Add(folder.Name);
        }

        return list;
    }

    public void SongDataSaveToJson(SongData data)
    {
        // Path Setting : Application.dataPath/SongDatas/[songName]/NotesData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        // Directory : SongDatas
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Directory : [songName]
        path = Path.Combine(path, GetComponent<Recorder>().songSelected);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // File : NotesData.txt
        path = Path.Combine(path, "SongData" + ".txt");

        // Data To Json String
        string jsonInfo = JsonUtility.ToJson(data, true);

        // Json String Save in text file
        File.WriteAllText(path, jsonInfo);

        Debug.Log("寫入完成");
        Debug.Log("dataPath: " + path);
    }

    public SongData SongDataLoadedFromJson(string name)
    {
        string loadData;

        //---------------Path---------------------

        // Path Setting : Application.dataPath/SongDatas/[songName]/NotesData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        // Directory : SongDatas
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Directory : [songName]
        path = Path.Combine(path, name);

        // File : NotesData.txt
        path = Path.Combine(path, "SongData" + ".txt");

        if (!File.Exists(path))
            return new SongData();

        //--------------------------------------

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<SongData>(loadData);

    }

    public void NotesDataSaveToJson(NotesData data)
    {
        // Path Setting : Application.dataPath/SongDatas/[songName]/NotesData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        // Directory : SongDatas
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Directory : [songName]
        path = Path.Combine(path, GetComponent<Recorder>().songSelected);

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

    public NotesData NotesDataLoadedFromJson(string name)
    {
        string loadData;

        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, name);

        path = Path.Combine(path, "NotesData" + ".txt");

        if (!File.Exists(path))
            return new NotesData();

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<NotesData>(loadData);

    }

    public IEnumerator SetAudioFromFileToConductor(string name)
    {

        // File to find : Application.dataPath/SongDatas/Gurenge/music.mp3
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, name);

        path = Path.Combine(path, "music.mp3");

#if UNITY_STANDALONE_OSX

      string url = "file://" + path;

#endif

#if UNITY_STANDALONE_LINUX

      string url = "file://" + path;

#endif

#if UNITY_STANDALONE_WIN

        string url = "file:///" + path;

#endif

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            // 等到處理完成再繼續
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                RecordConductor.instance.gameObject.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }

    }

    public void UpdateAllRecorder()
    {
        foreach(CircleNoteRecorder recorder in circleRecorders)
        {
            recorder.UpdateNote();
        }
        foreach (NoteRecorder recorder in noteRecorders)
        {
            recorder.UpdateNote();
        }
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

    List<float> ArrayToList(float[] arrayToList)
    {
        List<float> list = new List<float>();

        for(int i = 0; i < arrayToList.Length; i++)
        {
            list.Add(arrayToList[i]);
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

        //--------------------------------------------

        SongData data = new SongData();

        data.songName = GetComponent<Recorder>().songSelected;

        data.songLength = RecordConductor.instance.songAudioSource.clip.length;

        data.songDifficulty = SongDifficulty.Hard;

        data.maxCombo = MaxCombo();

        data.maxScore = MaxCombo() * 500;

        SongDataSaveToJson(data);

        //--------------------------------------------
}

    public void SetLoadedDataToAllRecorder()
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

    private int MaxCombo()
    {
        int maxCombo = 0;

        foreach(CircleNoteRecorder circleRecorder in circleRecorders)
        {
            maxCombo += circleRecorder.singleNote.Count;
        }
        foreach (NoteRecorder noteRecorder in noteRecorders)
        {
            maxCombo += noteRecorder.singleNote.Count;
            maxCombo += noteRecorder.longNoteStart.Count;
            maxCombo += noteRecorder.longNoteEnd.Count;
        }

        return maxCombo;
    }

}
