using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D playerRigidbody2D;
    float playerWalkSpeed = 10.0f;
    float playerRunSpeed = 20.0f;
    float jumpSpeed = 10.0f;
    [SerializeField] FloorCheck floorCheck;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        playerRigidbody2D.velocity = new Vector2(MoveX(), MoveY());
    }    
    float MoveX()
    {
        float velocityX;
        if(Input.GetKey(KeyCode.A))
        {
            velocityX = -playerWalkSpeed;
        } else if(Input.GetKey(KeyCode.D)) {
            velocityX = playerWalkSpeed;
        } else {
            velocityX = 0f;
        }
        return velocityX;
    }
    float MoveY()
    {
        float velocityY = playerRigidbody2D.velocity.y;
        
        if(Input.GetKeyDown(KeyCode.W) && floorCheck.getisOnFloor())
        {
            Debug.Log("aaa");
            velocityY = jumpSpeed;
        }
        return velocityY;
    }
}
