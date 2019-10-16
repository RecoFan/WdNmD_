using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player_transform;
    // Start is called before the first frame update
    [Space]
    public float half_camera_view_width = 4.9f;
    public float half_camera_view_height = 4.9f;
    [Space]
    public int frame_between_view = 10;

    private float esp = 0.001f;
    private Vector3 nowPosition;
    private Vector3 destPosition;
    private float yPerDis;
    private float xPerDis;

    private bool inMoving;

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
    }

    // Update is called once per frame
    void Update()
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

    void LateUpdate()
    {
        
    }
}
