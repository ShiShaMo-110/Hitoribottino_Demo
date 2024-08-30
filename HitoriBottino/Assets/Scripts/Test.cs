using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Camera mainCamera;
    Vector3 pos;
    Transform tra;
    // Start is called before the first frame update
    void Start()
    {
        tra = this.transform;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        tra.position = new Vector3(pos.x, pos.y, 0);
    }
}
