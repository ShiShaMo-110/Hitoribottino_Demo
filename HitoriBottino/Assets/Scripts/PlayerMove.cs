using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    static float speedInFloatArea = 0.3f;
    [SerializeField] FloorCheck floorCheck;
    [SerializeField] FloatAreaCheck floatAreaCheck;
    Camera mainCamera;
    bool isOnFloorTemp;
    bool isInFloatAreaTemp;

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
    public enum playerDirection
    {
        DIRECTION_RIGHT,
        DIRECTION_LEFT
    };
    public playerDirection playerDir;
    Rigidbody2D playerRigidbody2D;
    float playerSpeed;
    float playerWalkSpeed = 5.0f;
    float playerJumpSpeed = 10.0f;
    #endregion
    #region 無重力空間変数
    [SerializeField] GameObject floatAreaPrefab;
    int floatAreasize = 2;
    GameObject[] floatAreas;
    int floatAreasLength = 0;
    Vector3 cursorPosition;
    Vector3 cuttedCursorPositionTemp;
    Transform newFloatAreaTransform;
    Vector3 newFloatAreaPosition;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
//        isInFloatAreaTemp = floatAreaCheck.getisInFloatArea();
        if(isInFloatAreaTemp)
        {
            playerSpeed = speedInFloatArea;
        } else {
            playerSpeed = 1.0f;
        }
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
            velocityX = -playerWalkSpeed * playerSpeed;
            playerDir = playerDirection.DIRECTION_LEFT;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            velocityX = playerWalkSpeed * playerSpeed;
            playerDir = playerDirection.DIRECTION_RIGHT;
        }
        else
        {
            velocityX = 0f;
        }
        return velocityX;
    }
    #endregion
    #region 縦移動
    float MoveY()
    {
        float velocityY = playerRigidbody2D.velocity.y;
        isOnFloorTemp = floorCheck.getisOnFloor();
        if(Input.GetKeyDown(KeyCode.W) && isOnFloorTemp)
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
            cursorPosition = CuttingCursorPosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
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
    Vector3 CuttingCursorPosition(Vector3 cursorPosition)
    {
        cuttedCursorPositionTemp = new Vector3((int)cursorPosition.x, (int)cursorPosition.y, 0);
        return cuttedCursorPositionTemp;
    }
    #endregion

    void ChangePlayerState(playerState newPlayerState)
    {
        previousPlayerState = currentPlayerState;
        currentPlayerState = newPlayerState;
    }
}
