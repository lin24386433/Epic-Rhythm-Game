using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecorderCircleNote : MonoBehaviour
{
	// Keep a reference of the conductor.
	private RecordConductor conductor;

	[System.NonSerialized]
	public CircleNoteRecorder recorder;

	private bool moving = true;

	[System.NonSerialized]
	public float beat = 0f;

	private Transform startPos;

	private Transform endPos;

	private void Update()
	{
		Movement();

		MouseInput();

	}

	#region FUNC:Initialize(RecordConductor conductor, CircleNoteRecorder recorder, Transform startPoint, Transform endPoint, float beat)
	public void Initialize(RecordConductor conductor, CircleNoteRecorder recorder, Transform startPoint, Transform endPoint, float beat)
	{
		this.conductor = conductor;
		this.recorder = recorder;
		this.startPos = startPoint;
		this.endPos = endPoint;
		this.beat = beat;

		// Set to initial position.
		transform.position = startPoint.position;
		moving = true;
	}
	#endregion

	#region FUNC:Movement
	private void Movement()
	{
		if (moving)
		{
			transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));
		}
	}
	#endregion

	private void MouseInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

			if (hit == false)
				return;

			if (hit.collider.gameObject == this.gameObject && !GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.activeSelf && mousePos.y >= -2)
			{
				OnClicked();
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

			if (hit == false)
				return;

			if (hit.collider.gameObject == this.gameObject && mousePos.y >= -2)
			{
				recorder.DeleteNote(beat);
			}

		}
	}

	private void OnClicked()
	{
		GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.SetActive(true);
		GameObject.Find("Recorder").GetComponent<Recorder>().noteDetailWindow.GetComponent<NoteDetailWindow>().Init(this);
	}
}
