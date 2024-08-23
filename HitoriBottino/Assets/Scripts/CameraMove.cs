using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    #region プレイヤーに関する変数
    [SerializeField] GameObject player;
    Transform playerTransform;
    Vector3 playerPosition;
    PlayerMove playerMove;
    #endregion
    #region カメラに関する変数
    Transform cameraTransform;
    Vector3 currentCameraPosition;
    Vector3 goalCameraPosition;
    Vector3 posTemp;
    float cameraDistance = 5.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        cameraTransform = this.transform;
        playerMove = player.GetComponent<PlayerMove>();
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    #region 関数
    void MoveCamera()
    {
        playerPosition = playerTransform.position;
        currentCameraPosition = cameraTransform.position;
        if(playerMove.playerDir == PlayerMove.playerDirection.DIRECTION_RIGHT)
        {
            posTemp = playerPosition + Vector3.right * cameraDistance;
        }
        if(playerMove.playerDir == PlayerMove.playerDirection.DIRECTION_LEFT)
        {
            posTemp = playerPosition + Vector3.left * cameraDistance;
        }
        goalCameraPosition = new Vector3(posTemp.x, posTemp.y + 2.0f, currentCameraPosition.z);
        cameraTransform.position = Vector3.Lerp(currentCameraPosition, goalCameraPosition, 0.02f);
    }
    #endregion
}
