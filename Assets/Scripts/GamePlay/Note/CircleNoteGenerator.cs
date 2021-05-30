using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNoteGenerator : MonoBehaviour
{
	public List<float> singleNote;

	/*
	public float[] longNoteStart;
	public float[] longNoteEnd;
	*/

	// Next index for the array "singleNote".
	private int indexOfNextNote = 0;

	// Next index for the array "singleNote".
	//private int indexOfNextLongNote = 0;

	public Transform startPos;

	public Transform endPos;

	private void Update()
	{
		// Check if there are still notes in the track, and check if the next note is within the bounds we intend to show on screen.
		if (indexOfNextNote < singleNote.Count && singleNote[indexOfNextNote] < Conductor.instance.beatToShow)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			Note musicNote = ((GameObject)Instantiate(Conductor.instance.musicCircleNotePrefab, new Vector2(-100, -100), startPos.transform.rotation)).GetComponent<Note>();

			musicNote.Initialize(Conductor.instance, startPos, endPos, singleNote[indexOfNextNote]);

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
