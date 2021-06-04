using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class GamePlayDataController : MonoBehaviour
{
    // All Recorders 
    [SerializeField]
    private CircleNoteGenerator[] circleGenerator;

    [SerializeField]
    private NoteGenerator[] noteGenerator;

    [SerializeField]
    private Image bgImg;

    private NotesData notesDataToLoad;

    private void Awake()
    {
        StartCoroutine(LoadImageFromFile());

        StartCoroutine(LoadAudioFromFile());
        
        notesDataToLoad = NotesDataLoadedFromJson();


        SetLoadedDataToAllRecorder();

    }

    private void Start()
    {
        Conductor.instance.songBPM = SongDataLoadedFromJson(GameInfo.songName).songBPM;

        Conductor.instance.secPerBeat = 60 / Conductor.instance.songBPM;

        PlayerSetting playerSetting = PlayerSettingLoadedFromJson();

        switch (playerSetting.noteSpeed)
        {
            case 0:
                Conductor.instance.BeatsShownInAdvance = 8;
                break;
            case 1:
                Conductor.instance.BeatsShownInAdvance = 4;
                break;
            case 2:
                Conductor.instance.BeatsShownInAdvance = 2;
                break;
        }

        for(int i = 0; i < playerSetting.keyCodes.Length; i++)
        {
            GamePlayController.instance.keyCodes[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), playerSetting.keyCodes[i]);
        }

        

        //Conductor.instance.totalBeats = Conductor.instance.gameObject.GetComponent<AudioSource>().clip.length / Conductor.instance.secPerBeat;
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

    public PlayerSetting PlayerSettingLoadedFromJson()
    {
        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "Player.txt");

        if (!File.Exists(path))
        {
            PlayerSetting data = new PlayerSetting();

            // Data To Json String
            string jsonInfo = JsonUtility.ToJson(data, true);

            // Json String Save in text file
            File.WriteAllText(path, jsonInfo);

            return data;
        }

        string loadData;

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<PlayerSetting>(loadData);

    }

    private IEnumerator LoadAudioFromFile()
    {
        
        AudioClip myClip = null;

        // File to find : Application.dataPath/SongDatas/Gurenge/music.mp3
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, GameInfo.songName);

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
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                
            }
        }

        myClip.name = "music";

        Conductor.instance.gameObject.GetComponent<AudioSource>().clip = myClip;

        
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
                GameInfo.songImg = sprite;
            }
        }

    }


    private NotesData NotesDataLoadedFromJson()
    {
        string loadData;

        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, GameInfo.songName);

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
