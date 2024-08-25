using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAreaCheck : MonoBehaviour
{
    private bool isInFloatArea;
    private bool isEnter, isStay, isExit;

    public bool getisInFloatArea()
    {
        if(isEnter || isStay)
        {
            isInFloatArea = true;
        } else if (isExit) {
            isInFloatArea = false;
        }

        isEnter = false;
        isStay = false;
        isExit = false;
        return this.isInFloatArea;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FloatArea")
        {
            isEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "FloatArea")
        {
            isStay = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "FloatArea")
        {
            isExit = true;
        }
    }
}
