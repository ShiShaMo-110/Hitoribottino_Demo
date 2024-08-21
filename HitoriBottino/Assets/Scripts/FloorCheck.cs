using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    private bool isOnFloor;
    private bool isEnter, isStay, isExit;

    public bool getisOnFloor()
    {
        if(isEnter || isStay)
        {
            isOnFloor = true;
        } else if (isExit) {
            isOnFloor = false;
        }

        isEnter = false;
        isStay = false;
        isExit = false;
        return this.isOnFloor;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Floor")
        {
            isEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Floor")
        {
            isStay = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Floor")
        {
            isExit = true;
        }
    }
}
