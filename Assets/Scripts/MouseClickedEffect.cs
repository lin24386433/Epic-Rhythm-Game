using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickedEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Camera mainCamera = Camera.main;

            Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            pos.z = -2f;

            GameObject obj = Instantiate(effect, pos, Quaternion.identity);

            Destroy(obj, 1f);
        }
    }
}
