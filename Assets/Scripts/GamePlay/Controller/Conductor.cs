using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class Conductor : MonoBehaviour
{
	public static Conductor instance;

	// How many beats each minute last.
	public float songBPM;

	private float totalBeats;

	// How many seconds each beat last. This could be calculated by (60 / BPM).
	private float secPerBeat;

	[System.NonSerialized]
	public float songPosInBeats;

	// How many beats are contained on the screen. (Imagine this as "how many beats per bar" on music sheets.)
	public float BeatsShownInAdvance = 4f;

	// Some audio file might contain an empty interval at the start. We will substract this empty offset to calculate the actual position of the song.
	public float songOffset;

	// This plays the song
	private AudioSource songAudioSource;

	// Current song position. (We don't want to show this in Editor, hence the "NonSerialized")
	private float songposition;

	// To record the time passed of the audio engine in the last frame. We use this to calculate the position of the song.
	private float dsptimesong;

	[SerializeField]
	private float timeBeforeStart = 0f;

	[System.NonSerialized]
	public float beatToShow;

	[Space(10)]
	[Header("Spawn Prefab")]
	public GameObject musicNotePrefab;

	public GameObject musicLongNotePrefab;

	public GameObject musicCircleNotePrefab;

	void Awake()
	{
		if(instance == null)
			instance = this;

		StartCoroutine(LoadAudioFromFile());

		// Use AudioSettings.dspTime to get the accurate time passed for the audio engine.
		dsptimesong = (float)AudioSettings.dspTime;

		secPerBeat = 60 / songBPM;

		songAudioSource = GetComponent<AudioSource>();

		StartCoroutine(WaitForPlayTime(timeBeforeStart));
	}



	void Update()
	{
		songposition = (float)(AudioSettings.dspTime - dsptimesong - songOffset);

		//songPosInBeats = (songposition - timeBeforeStart) / secPerBeat;
		songPosInBeats = songAudioSource.time / secPerBeat;

		// Check if we need to instantiate a new note. (We obtain the current beat of the song by (songposition / secondsPerBeat).)
		// See the image for note spawning (note that the direction is reversed):
		// http://shinerightstudio.com/posts/music-syncing-in-rhythm-games/pic2.png
		beatToShow = songposition / secPerBeat + BeatsShownInAdvance;

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

		songAudioSource.clip = myClip;

		totalBeats = songAudioSource.clip.length / secPerBeat;
	}

	IEnumerator WaitForPlayTime(float time)
	{
		yield return new WaitForSeconds(time);
		// Play song.
		songAudioSource.Play();

		Invoke("SongFinished", songAudioSource.clip.length);
	}

	private void SongFinished()
    {
		GamePlayController.instance.SongFinished();

	}

}
