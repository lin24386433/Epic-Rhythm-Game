using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRecorder : MonoBehaviour
{
	public float[] singleNote;

	public float[] longNoteStart;
	public float[] longNoteEnd;

	// Next index for the array "singleNote".
	private int indexOfNextNote = 0;

	// Next index for the array "singleNote".
	private int indexOfNextLongNote = 0;

	[Space(10)]
	public Transform startPos;

	public Transform endPos;

	private void Update()
	{
		// Check if there are still notes in the track, and check if the next note is within the bounds we intend to show on screen.
		if (indexOfNextNote < singleNote.Length && singleNote[indexOfNextNote] < RecordConductor.instance.beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			RecorderNote musicNote = ((GameObject)Instantiate(RecordConductor.instance.musicNotePrefab, startPos.transform.position, startPos.transform.rotation)).GetComponent<RecorderNote>();

			musicNote.Initialize(RecordConductor.instance, startPos, endPos, singleNote[indexOfNextNote]);

			// Update the next index.
			indexOfNextNote++;
		}

		if (indexOfNextLongNote < longNoteStart.Length && longNoteStart[indexOfNextLongNote] < RecordConductor.instance.beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).


			RecorderLongNote longNote = Instantiate(RecordConductor.instance.musicLongNotePrefab, startPos.transform.position, startPos.transform.rotation).GetComponent<RecorderLongNote>();

			longNote.startNote.GetComponent<RecorderNote>().Initialize(RecordConductor.instance, startPos, endPos, longNoteStart[indexOfNextLongNote]);

			longNote.endNote.GetComponent<RecorderNote>().Initialize(RecordConductor.instance, startPos, endPos, longNoteEnd[indexOfNextLongNote]);

			longNote.Initialize(RecordConductor.instance, startPos, endPos);

			// Update the next index.
			indexOfNextLongNote++;
		}
	}
}
