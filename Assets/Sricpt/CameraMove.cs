using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Space]
    [Header("Player")]
    public Transform player_transform;
    private Vector3 lastPlayerPosition;
    // Start is called before the first frame update
    [Space]
    [Header("Camera (half) View Size")]
    public float half_camera_view_width = 4.9f;
    public float half_camera_view_height = 4.9f;
    [Space]
    [Header("Camera Move Speed Between Small Part")]
    public int frame_between_view = 10;
    [Space]
    [Header("Map Size")]
    public float mapLeftEdge = -13f;
    public float mapRightEdge = 66f;
    public float mapUpEdge = 14f;
    public float mapDownEdge = -4f;

    [Space]
    public int errorReason;

    private float esp = 0.001f;
    private Vector3 nowPosition;
    private Vector3 destPosition;
    private float yPerDis;
    private float xPerDis;

    private bool inMoving;
    private bool isNeedFollow; //weather the camera needs to follow the player
    private int movingDirection; // 1:left 2:right 3:up 4:d

    private float abs(float a)
    {
        if (a >= 0) return a;
        else return -a;
    }

    private bool inCameraView()
    {
        Vector3 playerPos = player_transform.position;
        if (abs(playerPos.x - nowPosition.x) <= half_camera_view_width && abs(playerPos.y - nowPosition.y) <= half_camera_view_height) return true;
        else return false;
    }

    void getPerDis() {
        Vector3 playerPos = player_transform.position;
        destPosition = nowPosition;
        if (playerPos.y - nowPosition.y > half_camera_view_height) //UP
        {
            yPerDis = 2 * half_camera_view_height / frame_between_view;
            xPerDis = 0;
            destPosition.y = destPosition.y + 2 * half_camera_view_height;
        }
        else if (playerPos.y - nowPosition.y < -half_camera_view_height) //DOWN
        {
            yPerDis = -2 * half_camera_view_height / frame_between_view;
            xPerDis = 0;
            destPosition.y = destPosition.y - 2 * half_camera_view_height;
        }
        else if (playerPos.x - nowPosition.x > half_camera_view_width) //RIGHT
        {
            yPerDis = 0;
            xPerDis = 2 * half_camera_view_width / frame_between_view;
            destPosition.x = destPosition.x + 2 * half_camera_view_width;
        }
        else if (playerPos.x - nowPosition.x < -half_camera_view_width) //LEFT
        {
            yPerDis = 0;
            xPerDis = -2 * half_camera_view_width / frame_between_view;
            destPosition.x = destPosition.x - 2 * half_camera_view_width;
        }
    }

    void Start()
    {
        nowPosition = transform.position;
        half_camera_view_height = GetComponent<Camera>().orthographicSize;
        half_camera_view_width = half_camera_view_height * UnityEngine.Screen.width / UnityEngine.Screen.height;
        inMoving = false;

        isNeedFollow = true;
        Vector3 playerPos = player_transform.position;
        playerPos.x = nowPosition.x;
        player_transform.position = playerPos; //player is located under the camera from the beginning (middle of the map).
        //lastPlayerPosition = player_transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (isNeedFollow == true) 
        {
            //---------camera needs to follow the player-------
            Vector3 tempPosition = transform.position;
            tempPosition.x = player_transform.position.x;
            transform.position = tempPosition;
            //------------------------------------------------------
        }*/
        if (isNeedFollow == false)
        {
            if (!inMoving)
            {
                if (!inCameraView())
                {
                    getPerDis();
                    inMoving = true;
                }
            }
            else
            {
                if (abs(nowPosition.x - destPosition.x) < esp && abs(nowPosition.y - destPosition.y) < esp)
                {
                    inMoving = false;
                }
                else
                {
                    nowPosition.x += xPerDis;
                    nowPosition.y += yPerDis;
                    transform.position = nowPosition;
                }
            }
        }
        else
        {
            nowPosition = transform.position;
        }
        if (mapRightEdge - player_transform.position.x < half_camera_view_width || player_transform.position.x - mapLeftEdge < half_camera_view_width)
        {
            errorReason = 1;
            isNeedFollow = false;
        }
        else if (player_transform.position.x - half_camera_view_width < mapLeftEdge || player_transform.position.x + half_camera_view_width > mapRightEdge)
        {
            errorReason = 2;
            isNeedFollow = false;
        }
        else if (inMoving == true)
        {
            errorReason = 3;
            isNeedFollow = false;
        }
        else
        {
            errorReason = 5;
            isNeedFollow = true;
        }
    }

    void LateUpdate()
    {
        /*if (player_transform.position.x - lastPlayerPosition.x > 0)
        {
            movingDirection = 2; //Right
        }
        else if (player_transform.position.x - lastPlayerPosition.x < 0)
        {
            movingDirection = 1; //Left
        }
        lastPlayerPosition = player_transform.position;*/
        if (isNeedFollow == true)
        {
            //---------camera needs to follow the player-------
            Vector3 tempPosition = transform.position;
            tempPosition.x = player_transform.position.x;
            transform.position = tempPosition;
            //------------------------------------------------------
        }
    }
}
