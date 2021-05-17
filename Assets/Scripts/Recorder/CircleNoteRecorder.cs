using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNoteRecorder : MonoBehaviour
{
	public float[] singleNote;

	/*
	public float[] longNoteStart;
	public float[] longNoteEnd;
	*/

	// Next index for the array "singleNote".
	//private int indexOfNextLongNote = 0;

	private GameObject notesPool;

	public Transform startPos;

	public Transform endPos;

    private void Start()
    {
		SpawnNotes();
    }

    private void Update()
	{
		
	}

	private void SpawnNotes()
    {
		notesPool = new GameObject(this.gameObject.name + " NotesPool");

		// Next index for the array "singleNote".
		int indexOfNextNote = 0;

		// Check if there are still notes in the track, and check if the next note is within the bounds we intend to show on screen.
		while (indexOfNextNote < singleNote.Length)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).

			RecorderNote musicNote = Instantiate(RecordConductor.instance.musicCircleNotePrefab, startPos.transform.position, startPos.transform.rotation).GetComponent<RecorderNote>();

			musicNote.Initialize(RecordConductor.instance, startPos, endPos, singleNote[indexOfNextNote]);

			musicNote.transform.SetParent(notesPool.transform);

			// Update the next index.
			indexOfNextNote++;
		}
		/*
		if (indexOfNextLongNote < longNoteStart.Length && longNoteStart[indexOfNextLongNote] < Conductor.instance.beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).


			LongNote longNote = Instantiate(Conductor.instance.musicLongNotePrefab, startPos.transform.position, startPos.transform.rotation).GetComponent<LongNote>();

			longNote.startNote.GetComponent<Note>().Initialize(Conductor.instance, startPos, endPos, longNoteStart[indexOfNextLongNote]);

			longNote.endNote.GetComponent<Note>().Initialize(Conductor.instance, startPos, endPos, longNoteEnd[indexOfNextLongNote]);

			longNote.Initialize(Conductor.instance, startPos, endPos);

			// Update the next index.
			indexOfNextLongNote++;
		}
		*/
	}
}
