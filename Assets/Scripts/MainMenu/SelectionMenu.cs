using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    RectTransform rect;

    public float angle = 0f;

    public float speed = 1f;

    public GameObject[] menuBars;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        angle += Time.deltaTime * speed;

        this.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        foreach(GameObject obj in menuBars)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
