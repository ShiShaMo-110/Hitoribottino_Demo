using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    static float speedInFloatArea = 0.3f;
    [SerializeField] FloorCheck floorCheck;
    Camera mainCamera = Camera.main;

    #region プレイヤー変数
    enum playerState {
        STATE_WALK,
        STATE_RUN,
        STATE_PUTFroatArea,
        STATE_JUMP,
        STATE_ATTACK,
        STATE_JUMPATTACK
    };
    playerState currentPlayerState;
    playerState previousPlayerState;
    Rigidbody2D playerRigidbody2D;
    float playerWalkSpeed = 10.0f;
    float playerRunSpeed = 20.0f;
    float playerJumpSpeed = 10.0f;
    #endregion
    #region 無重力空間変数
    [SerializeField] GameObject floatAreaPrefab;
    GameObject[] floatAreas;
    int floatAreasLength = 0;
    Vector3 cursorPosition;
    Transform newFloatAreaTransform;
    Vector3 newFloatAreaPosition;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
        floatAreas = new GameObject[floatAreasLength + 1];
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        PutFloatArea();
    }

    #region プレイヤー移動
    void MovePlayer()
    {
        playerRigidbody2D.velocity = new Vector2(MoveX(), MoveY());
        if(currentPlayerState == playerState.STATE_JUMP && floorCheck.getisOnFloor())
        {
            ChangePlayerState(playerState.STATE_WALK);
        }
    }    

    #region 横移動
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
    #endregion
    #region 縦移動
    float MoveY()
    {
        float velocityY = playerRigidbody2D.velocity.y;
        Debug.Log(floorCheck.getisOnFloor());
        if(Input.GetKeyDown(KeyCode.W) && floorCheck.getisOnFloor())
        {
            previousPlayerState = currentPlayerState;
            currentPlayerState = playerState.STATE_JUMP;
            velocityY = playerJumpSpeed;
        }
        return velocityY;
    }
    #endregion
    #endregion

    #region 無重力空間

    void PutFloatArea()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 0f;
            currentPlayerState = playerState.STATE_PUTFroatArea;
            InstantiateFloatArea();
            newFloatAreaTransform = floatAreas[floatAreasLength].transform;
        }
        if(Input.GetMouseButton(1))
        {
            cursorPosition = mainCamera.ViewportToWorldPoint(Input.mousePosition);
            newFloatAreaTransform.position = cursorPosition;
        }
        if(Input.GetMouseButtonUp(1))
        {
            Time.timeScale = 1.0f;
            currentPlayerState = previousPlayerState;
        }
    }

    void InstantiateFloatArea()
    {
        Destroy(floatAreas[0]);
        for(int i = 0; i < floatAreasLength; i ++)
        {
            floatAreas[i] = floatAreas[i + 1];
        }
        floatAreas[floatAreasLength] = Instantiate(floatAreaPrefab, cursorPosition, Quaternion.identity);
    }
    #endregion

    void ChangePlayerState(playerState newPlayerState)
    {
        previousPlayerState = currentPlayerState;
        currentPlayerState = newPlayerState;
    }
}
