using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
	// Keep a reference of the conductor.
	public GameController conductor;

	public Transform startPos;

	public Transform endPos;

	public float beat;

	public void Initialize(GameController conductor, Transform startPoint, Transform endPoint, float beat)
	{
		this.conductor = conductor;
		this.startPos = startPoint;
		this.endPos = endPoint;
		this.beat = beat;

		// Set to initial position.
		transform.position = startPoint.position;

	}

	void Update()
	{

		transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));

	}
}
