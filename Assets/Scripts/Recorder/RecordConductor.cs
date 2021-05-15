using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordConductor : MonoBehaviour
{
	public static RecordConductor instance;

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

	[System.NonSerialized]
	// This plays the song. 
	public AudioSource songAudioSource;

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


	void Start()
	{
		if (instance == null)
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
		songposition = (float)(AudioSettings.dspTime - dsptimesong - songOffset);

		songPosInBeats = (songposition - timeBeforeStart) / secPerBeat;

		beatToShow = songposition / secPerBeat + BeatsShownInAdvance;

	}


	IEnumerator WaitForPlayTime(float time)
	{
		yield return new WaitForSeconds(time);
		// Play song.
		songAudioSource.Play();
	}
}
