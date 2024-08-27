using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] FloorCheck floorCheck;
    [SerializeField] FloatAreaCheck floatAreaCheck_body;
    [SerializeField] FloatAreaCheck floatAreaCheck_head;
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
    bool isCanJump;
    #endregion
    #region 無重力空間変数
    [SerializeField] GameObject floatAreaPrefab;
    int floatAreasize = 2;
    GameObject[] floatAreas;
    int floatAreasLength = 0;
    Vector3 cursorPosition;
    Vector3 cuttedCursorPositionTemp;
    Transform newFloatAreaTransform;
    Vector3 playerVelocityInFloatArea;
    Vector3 playerVelocityInFloatAreaTemp;
    static float speedInFloatArea = 0.5f;
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
        InFloatArea();
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
            playerDir = playerDirection.DIRECTION_LEFT;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            velocityX = playerWalkSpeed;
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
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (isCanJump || isOnFloorTemp)
            {
                previousPlayerState = currentPlayerState;
                currentPlayerState = playerState.STATE_JUMP;
                velocityY = playerJumpSpeed;
                isCanJump = false;
            }
        }
        return velocityY;
    }
    #endregion
    #endregion

    #region 無重力空間設置

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
        cuttedCursorPositionTemp = new Vector3(cursorPosition.x, cursorPosition.y, 0);
        return cuttedCursorPositionTemp;
    }
    #endregion
    #region 無重力空間内
    void InFloatArea()
    {
        if(floatAreaCheck_body.getisEnter())
        {
            playerVelocityInFloatAreaTemp = playerRigidbody2D.velocity;
            isCanJump = true;
            if(floatAreaCheck_head.getisInFloatArea())
            {
                playerVelocityInFloatArea = playerVelocityInFloatAreaTemp * speedInFloatArea;
            } else {
                playerVelocityInFloatArea = playerVelocityInFloatAreaTemp * speedInFloatArea;
            }
        }
        if(floatAreaCheck_body.getisInFloatArea())
        {
            playerRigidbody2D.velocity = playerVelocityInFloatArea;
        }
        if(floatAreaCheck_body.getisExit())
        {
            Debug.Log("Exit");
            playerRigidbody2D.velocity = playerVelocityInFloatAreaTemp;
        }
    }
    #endregion
    void ChangePlayerState(playerState newPlayerState)
    {
        previousPlayerState = currentPlayerState;
        currentPlayerState = newPlayerState;
    }
}
