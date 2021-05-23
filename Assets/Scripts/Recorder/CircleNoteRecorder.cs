using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNoteRecorder : MonoBehaviour
{
	// Editable in editor
	public List<float> singleNote;

	[Space(10)]
	[SerializeField]
	private Transform startPos;

	[SerializeField]
	private Transform endPos;

	[SerializeField]
	private KeyCode keyToPress;

	// Ineditable in editor
	private GameObject notesPool;

	private void Start()
	{
		SpawnNotes();
	}

	private void Update()
	{
		AddNote();
	}

	#region FUNC:SpawnNotes
	private void SpawnNotes()
	{
		notesPool = new GameObject(this.gameObject.name + " NotesPool");

		// Next index for the array "singleNote".
		int indexOfNextNote = 0;

		// Check if there are still notes in the track, and check if the next note is within the bounds we intend to show on screen.
		while (indexOfNextNote < singleNote.Count)
		{

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			RecorderCircleNote musicNote = ((GameObject)Instantiate(RecordConductor.instance.musicCircleNotePrefab, startPos.transform.position, startPos.transform.rotation)).GetComponent<RecorderCircleNote>();

			musicNote.Initialize(RecordConductor.instance, this, startPos, endPos, singleNote[indexOfNextNote]);

			musicNote.transform.SetParent(notesPool.transform);

			// Update the next index.
			indexOfNextNote++;
		}

	}
	#endregion

	public void UpdateNote()
	{
		Destroy(notesPool);
		SpawnNotes();
	}

	#region FUNC:AddNote
	private void AddNote()
	{
		if (Input.GetKeyDown(keyToPress) && BeatNowAvaliable(RecordConductor.instance.songPosInBeats))
		{
			Destroy(notesPool);
			singleNote.Add(RecordConductor.instance.songPosInBeats);
			SpawnNotes();
		}
	}
	#endregion

	#region FUNC:DeleteNote(float beat)
	public void DeleteNote(float beat)
	{
		for (int i = 0; i < singleNote.Count; i++)
		{
			if (beat == singleNote[i])
				singleNote.Remove(beat);
		}
		Destroy(notesPool);
		SpawnNotes();
	}
	#endregion

	#region FUNC:BeatNowAvaliable(float beatNow)
	private bool BeatNowAvaliable(float beatNow)
	{
		foreach (float f in singleNote)
		{
			if (f == beatNow)
				return false;
		}

		return true;
	}
    #endregion
}
