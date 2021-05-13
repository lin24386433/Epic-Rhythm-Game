using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	// The beat-locations of all music notes in the song should be entered in this array in Editor.
	// See the image: http://shinerightstudio.com/posts/music-syncing-in-rhythm-games/pic1.png
	public float[] singleNote;

	public float[] longNoteStart;
	public float[] longNoteEnd;

	// How many beats each minute last.
	public float songBPM;

	public float totalBeats;

	// How many seconds each beat last. This could be calculated by (60 / BPM).
	public float secPerBeat;

	public float songPosInBeats;

	// How many beats are contained on the screen. (Imagine this as "how many beats per bar" on music sheets.)
	public float BeatsShownInAdvance = 4f;

	// Some audio file might contain an empty interval at the start. We will substract this empty offset to calculate the actual position of the song.
	public float songOffset;

	// This plays the song.
	public AudioSource songAudioSource;

	// Current song position. (We don't want to show this in Editor, hence the "NonSerialized")
	public float songposition;

	// Next index for the array "singleNote".
	private int indexOfNextNote = 0;

	// Next index for the array "singleNote".
	private int indexOfNextLongNote = 0;

	// To record the time passed of the audio engine in the last frame. We use this to calculate the position of the song.
	private float dsptimesong;

	[SerializeField]
	private float timeBeforeStart = 0f;

	public GameObject musicNotePrefab;

	public GameObject musicLongNotePrefab;

	public GameObject musicLongNoteStartPrefab;

	public GameObject musicLongNoteEndPrefab;

	public Transform startPos;

	public Transform endPos;

	public Text txt;

	float beatToShow;

	public static GameController instance;

	void Start()
    {
		instance = this;

		// Use AudioSettings.dspTime to get the accurate time passed for the audio engine.
		dsptimesong = (float)AudioSettings.dspTime;

		secPerBeat = 60 / songBPM;

		songAudioSource = GetComponent<AudioSource>();

		totalBeats = songAudioSource.clip.length / secPerBeat;

		StartCoroutine(WaitForPlayTime(timeBeforeStart));
	}

	void Update()
    {
		txt.text = songPosInBeats.ToString();

		songposition = (float)(AudioSettings.dspTime - dsptimesong - songOffset);

		songPosInBeats = (songposition - timeBeforeStart) / secPerBeat;

		// Check if we need to instantiate a new note. (We obtain the current beat of the song by (songposition / secondsPerBeat).)
		// See the image for note spawning (note that the direction is reversed):
		// http://shinerightstudio.com/posts/music-syncing-in-rhythm-games/pic2.png
		beatToShow = songposition / secPerBeat + BeatsShownInAdvance;
		
		// Check if there are still notes in the track, and check if the next note is within the bounds we intend to show on screen.
		if (indexOfNextNote < singleNote.Length && singleNote[indexOfNextNote] < beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			Note musicNote = ((GameObject)Instantiate(musicNotePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Note>();

			musicNote.Initialize(this, startPos, endPos, singleNote[indexOfNextNote]);

			// Update the next index.
			indexOfNextNote++;
		}

		if (indexOfNextLongNote < longNoteStart.Length && longNoteStart[indexOfNextLongNote] < beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			Note musicNoteStart = ((GameObject)Instantiate(musicLongNoteStartPrefab, new Vector2(100, 0), Quaternion.identity)).GetComponent<Note>();

			musicNoteStart.Initialize(this, startPos, endPos, longNoteStart[indexOfNextLongNote]);

			Note musicNoteEnd = ((GameObject)Instantiate(musicLongNoteEndPrefab, new Vector2(100, 0), Quaternion.identity)).GetComponent<Note>();

			musicNoteEnd.Initialize(this, startPos, endPos, longNoteEnd[indexOfNextLongNote]);

			LongNote longNote = Instantiate(musicLongNotePrefab, new Vector2(100, 0), Quaternion.identity).GetComponent<LongNote>();

			longNote.startNote = musicNoteStart.gameObject;

			longNote.endNote = musicNoteEnd.gameObject;

			float longNoteLength = longNoteEnd[indexOfNextLongNote] - longNoteStart[indexOfNextLongNote];

			Debug.Log(longNoteLength);

			// Update the next index.
			indexOfNextLongNote++;
		}

	}

	IEnumerator WaitForPlayTime(float time)
    {
		yield return new WaitForSeconds(time);
		// Play song.
		songAudioSource.Play();
	}

}


