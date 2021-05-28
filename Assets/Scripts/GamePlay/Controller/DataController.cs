using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;


public class DataController : MonoBehaviour
{
    // All Recorders 
    [SerializeField]
    private CircleNoteGenerator[] circleGenerator;

    [SerializeField]
    private NoteGenerator[] noteGenerator;

    [SerializeField]
    private Image bgImg;

    public string songName = "Gurenge";

    // Save Datas
    private NotesData notesDataToSave;

    private NotesData notesDataToLoad;

    private void Start()
    {
        StartCoroutine(LoadImageFromFile());

        notesDataToLoad = NotesDataLoadedFromJson();

        SetLoadedDataToAllRecorder();

    }

    private IEnumerator LoadImageFromFile()
    {

        // File to find : Application.dataPath/SongDatas/Gurenge/music.mp3
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, GameInfo.songName);

        path = Path.Combine(path, "bg.jpg");

#if UNITY_STANDALONE_OSX

        string url = "file://" + path;

#endif

#if UNITY_STANDALONE_LINUX

        string url = "file://" + path;

#endif

#if UNITY_STANDALONE_WIN

        string url = "file:///" + path;

#endif

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                Texture2D myImg = texture;
                Sprite sprite = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), Vector2.zero);
                bgImg.sprite = sprite;
       
            }
        }

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

    T[] ListToArray<T>(List<T> listToArray)
    {
        T[] array = new T[listToArray.Count];
        for (int i = 0; i < listToArray.Count; i++)
        {
            array[i] = listToArray[i];
        }
        return array;
    }

    List<T> ArrayToList<T>(T[] arrayToList)
    {
        List<T> list = new List<T>();

        foreach (T t in arrayToList)
        {
            list.Add(t);
        }

        return list;
    }

    private void OnSaveBtnClicked()
    {
        notesDataToSave = new NotesData();

        #region data
        notesDataToSave.circleNote_Single1 = ListToArray(circleGenerator[0].singleNote);
        notesDataToSave.circleNote_Single2 = ListToArray(circleGenerator[1].singleNote);
        notesDataToSave.circleNote_Single3 = ListToArray(circleGenerator[2].singleNote);
        notesDataToSave.circleNote_Single4 = ListToArray(circleGenerator[3].singleNote);
        notesDataToSave.circleNote_Single5 = ListToArray(circleGenerator[4].singleNote);

        notesDataToSave.singleNote1 = ListToArray(noteGenerator[0].singleNote);
        notesDataToSave.longNoteStart1 = ListToArray(noteGenerator[0].longNoteStart);
        notesDataToSave.longNoteEnd1 = ListToArray(noteGenerator[0].longNoteEnd);

        notesDataToSave.singleNote2 = ListToArray(noteGenerator[1].singleNote);
        notesDataToSave.longNoteStart2 = ListToArray(noteGenerator[1].longNoteStart);
        notesDataToSave.longNoteEnd2 = ListToArray(noteGenerator[1].longNoteEnd);

        notesDataToSave.singleNote3 = ListToArray(noteGenerator[2].singleNote);
        notesDataToSave.longNoteStart3 = ListToArray(noteGenerator[2].longNoteStart);
        notesDataToSave.longNoteEnd3 = ListToArray(noteGenerator[2].longNoteEnd);

        notesDataToSave.singleNote4 = ListToArray(noteGenerator[3].singleNote);
        notesDataToSave.longNoteStart4 = ListToArray(noteGenerator[3].longNoteStart);
        notesDataToSave.longNoteEnd4 = ListToArray(noteGenerator[3].longNoteEnd);

        notesDataToSave.singleNote5 = ListToArray(noteGenerator[4].singleNote);
        notesDataToSave.longNoteStart5 = ListToArray(noteGenerator[4].longNoteStart);
        notesDataToSave.longNoteEnd5 = ListToArray(noteGenerator[4].longNoteEnd);
        #endregion

        NotesDataSaveToJson(notesDataToSave);
    }

    private void SetLoadedDataToAllRecorder()
    {
        circleGenerator[0].singleNote = ArrayToList(notesDataToLoad.circleNote_Single1);
        circleGenerator[1].singleNote = ArrayToList(notesDataToLoad.circleNote_Single2);
        circleGenerator[2].singleNote = ArrayToList(notesDataToLoad.circleNote_Single3);
        circleGenerator[3].singleNote = ArrayToList(notesDataToLoad.circleNote_Single4);
        circleGenerator[4].singleNote = ArrayToList(notesDataToLoad.circleNote_Single5);

        noteGenerator[0].singleNote = ArrayToList(notesDataToLoad.singleNote1);
        noteGenerator[0].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart1);
        noteGenerator[0].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd1);

        noteGenerator[1].singleNote = ArrayToList(notesDataToLoad.singleNote2);
        noteGenerator[1].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart2);
        noteGenerator[1].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd2);

        noteGenerator[2].singleNote = ArrayToList(notesDataToLoad.singleNote3);
        noteGenerator[2].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart3);
        noteGenerator[2].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd3);

        noteGenerator[3].singleNote = ArrayToList(notesDataToLoad.singleNote4);
        noteGenerator[3].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart4);
        noteGenerator[3].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd4);

        noteGenerator[4].singleNote = ArrayToList(notesDataToLoad.singleNote5);
        noteGenerator[4].longNoteStart = ArrayToList(notesDataToLoad.longNoteStart5);
        noteGenerator[4].longNoteEnd = ArrayToList(notesDataToLoad.longNoteEnd5);

    }
}
