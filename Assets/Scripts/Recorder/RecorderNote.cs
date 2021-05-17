using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecorderNote : MonoBehaviour
{
	// Keep a reference of the conductor.
	private RecordConductor conductor;

	public bool moving = true;

	public float beat = 0f;

	[SerializeField]
	private Transform startPos;

	[SerializeField]
	private Transform endPos;


	[Space(20)]
	[SerializeField]
	private UnityEvent OnClick = new UnityEvent();

	private void Start()
    {
		OnClick.AddListener(OnClicked);
    }

    public void Initialize(RecordConductor conductor, Transform startPoint, Transform endPoint, float beat)
	{
		this.conductor = conductor;
		this.startPos = startPoint;
		this.endPos = endPoint;
		this.beat = beat;

		// Set to initial position.
		transform.position = startPoint.position;
		moving = true;
	}

	void Update()
	{

		if (Input.GetMouseButtonDown(0))  
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

			if (hit == false)
				return;

			if (hit.collider.gameObject == this.gameObject)
			{
				Debug.Log("Button Clicked");
				OnClick.Invoke();
            }

		}

		if (moving)
		{
			transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));
		}

	}

	private void OnClicked()
    {

    }


}
