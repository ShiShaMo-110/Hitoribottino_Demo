using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
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
    enum playerState
    {
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
    Transform playerTransform;
    Vector3 playerPosition;
    float playerSpeed;
    float playerWalkSpeed = 5.0f;
    float playerJumpSpeed = 10.0f;
    bool isCanJump;
    Vector3 playerVelocityTemp;
    #endregion
    #region 無重力空間変数
    [SerializeField] GameObject floatAreaPrefab;
    int floatAreasize = 3;
    GameObject[] floatAreas;
    int floatAreasLength = 2;
    int floatAreaCoolTime = 0;
    Vector3 cursorPosition;
    Vector3 cuttedCursorPositionTemp;
    Transform newFloatAreaTransform;
    Vector3 playerVelocityInFloatArea;
    Vector3 playerVelocityInFloatAreaTemp;
    bool isPuttingFloatArea;
    #endregion
    #region 攻撃判定
    [SerializeField] GameObject bullet_up;
    [SerializeField] GameObject bullet_down;
    [SerializeField] GameObject bullet_left;
    [SerializeField] GameObject bullet_right;
    Vector3 directionTemp;
    bool isAttacking;
    int bulletCoolTime = 50;
    int bulletTime = 0;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
        floatAreas = new GameObject[floatAreasLength + 1];
        playerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetCursorPosition();
        MovePlayer();
        PutFloatArea();
        InFloatArea();
        ShootBullet();
        playerRigidbody2D.velocity = playerVelocityTemp;
    }

    #region プレイヤー移動
    void MovePlayer()
    {
        playerVelocityTemp = new Vector2(MoveX(), MoveY());
        if (currentPlayerState == playerState.STATE_JUMP && floorCheck.getisOnFloor())
        {
            ChangePlayerState(playerState.STATE_WALK);
        }
    }

    #region 横移動
    float MoveX()
    {
        float velocityX;
        if (Input.GetKey(KeyCode.A))
        {
            velocityX = -playerWalkSpeed;
            playerDir = playerDirection.DIRECTION_LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKeyDown(KeyCode.W))
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
    #region 弾発射
    void ShootBullet()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = !isAttacking;
        }
        if(isAttacking)
        {
            if(bulletTime > bulletCoolTime)
            {
                Instantiate(BulletDirection(), playerTransform.position, Quaternion.identity);
                bulletTime = 0;
            } else {
                bulletTime ++;
            }
        }
    }
    GameObject BulletDirection()
    {
        cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        playerPosition = playerTransform.position;
        directionTemp = cursorPosition - playerPosition;
        switch((directionTemp.x > directionTemp.y, -directionTemp.x > directionTemp.y))
        {
            case(true,true):
                return bullet_down;
            case(true,false):
                return bullet_right;
            case(false,true):
                return bullet_left;
            case(false,false):
                return bullet_up;
        }
    }
    #endregion
    #region 無重力空間設置

    void PutFloatArea()
    {
        floatAreaCoolTime++;
        if (floatAreaCoolTime > 500)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 0f;
                currentPlayerState = playerState.STATE_PUTFroatArea;
                InstantiateFloatArea();
                newFloatAreaTransform = floatAreas[floatAreasLength].transform;
                isPuttingFloatArea = true;
            }
            if (isPuttingFloatArea)
            {
                newFloatAreaTransform.position = CuttingCursorPosition(cursorPosition);
            }
            if (Input.GetMouseButtonUp(1))
            {
                Time.timeScale = 1.0f;
                currentPlayerState = previousPlayerState;
                floatAreaCoolTime = 0;
                isPuttingFloatArea = false;
            }
        }
    }

    void InstantiateFloatArea()
    {
        Destroy(floatAreas[0]);
        for (int i = 0; i < floatAreasLength; i++)
        {
            floatAreas[i] = floatAreas[i + 1];
        }
        floatAreas[floatAreasLength] = Instantiate(floatAreaPrefab, cursorPosition, Quaternion.identity);
    }
    Vector3 CuttingCursorPosition(Vector3 cursorPosition)
    {
        cuttedCursorPositionTemp = new Vector3(cursorPosition.x, cursorPosition.y, 0);
        cuttedCursorPositionTemp = cuttedCursorPositionTemp + new Vector3(-(cuttedCursorPositionTemp.x % floatAreasize) + floatAreasize/2f, -(cuttedCursorPositionTemp.y % floatAreasize) + floatAreasize/2f, 0);
        return cuttedCursorPositionTemp;
    }
    #endregion
    #region 無重力空間内
    void InFloatArea()
    {
        if (floatAreaCheck_body.getisEnter())
        {
            playerVelocityInFloatAreaTemp = playerRigidbody2D.velocity;
            isCanJump = true;
            if (floatAreaCheck_head.getisInFloatArea())
            {
                playerVelocityInFloatArea = playerVelocityInFloatAreaTemp * FloatAreaCheck.speedInFloatArea;
            }
            else
            {
                //仮
                playerVelocityInFloatArea = playerVelocityInFloatAreaTemp * FloatAreaCheck.speedInFloatArea;
            }
        }
        if (floatAreaCheck_body.getisInFloatArea())
        {
            if(playerRigidbody2D.velocity.x == 0)
            {
                playerVelocityInFloatArea = new Vector3(-playerVelocityInFloatArea.x, playerVelocityInFloatArea.y, 0);
            }
            if(playerRigidbody2D.velocity.y == 0)
            {
                playerVelocityInFloatArea = new Vector3(playerVelocityInFloatArea.x, -playerVelocityInFloatArea.y, 0);
            }
            playerVelocityTemp = playerVelocityInFloatArea;
        }
        if (floatAreaCheck_body.getisExit())
        {
            Debug.Log("Exit");
            playerVelocityTemp = playerVelocityInFloatAreaTemp;
        }
    }
    #endregion
    void ChangePlayerState(playerState newPlayerState)
    {
        previousPlayerState = currentPlayerState;
        currentPlayerState = newPlayerState;
    }
    void GetCursorPosition()
    {
        cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Game Over");
        }
    }
}
