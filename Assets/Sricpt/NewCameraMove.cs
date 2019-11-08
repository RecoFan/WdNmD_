﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMove : MonoBehaviour
{
    public float[][] stageList;
    private int stageCount = 8;

    [Space]
    [Header("Player")]
    public Transform playerTransform;
    private Vector3 lastPlayerPosition;

    [Space]
    [Header("Move Speed between Stage")]
    public int frameBetweenView = 10;
    //----------Destination------------------
    private float eps = 0.001f;
    private Vector3 destPosition;
    private float xPerStep;
    private float yPerStep;
    //-----------------------------------------

    [Space]
    [Header("Please Ignore")]
    //------View Size of Camera-----------
    private float halfCameraViewWidth; //xLeft,xRight,yUp,yDown
    private float halfCameraViewHeight;
    public int nowMapIndex;
    public int lastMapIndex;
    private float mapLeftEdge;
    private float mapRightEdge;
    private float mapUpEdge;
    private float mapDownEdge;
    //-----------------------------------------
    public float leftUpDis;
    public float rightUpDis;
    public float leftDownDis;
    public float rightDownDis;

    private bool isCameraFollowPlayer;
    private int moveType; //1:Left&Right 2:Up&Down
    private bool isCameraSwitch;

    // Start is called before the first frame update
    void Start()
    {
        stageList = new float[8][];
        stageList[0] = new float[] { -13f, 25f, 14f, -3f}; //xLeft,xRight,yUp,yDown
        stageList[1] = new float[] { 24f, 64f, 16f, -1f };
        stageList[2] = new float[] { 0, 0, 0, 0 };
        stageList[3] = new float[] { 0, 0, 0, 0 };
        stageList[4] = new float[] { 0, 0, 0, 0 };
        stageList[5] = new float[] { 0, 0, 0, 0 };
        stageList[6] = new float[] { 0, 0, 0, 0 };
        stageList[7] = new float[] { 0, 0, 0, 0 };
    
        halfCameraViewHeight = GetComponent<Camera>().orthographicSize;
        halfCameraViewWidth = halfCameraViewHeight * UnityEngine.Screen.width / UnityEngine.Screen.height;

        isCameraFollowPlayer = true;
        isCameraSwitch = false;
        Vector3 tempVector = playerTransform.position;
        tempVector.x = transform.position.x;
        playerTransform.position = tempVector;

        lastPlayerPosition = playerTransform.position;

        int overlapCount = getMapCount(playerTransform.position);
        if(overlapCount == 1) //must
        {
            lastMapIndex = nowMapIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCameraFollowPlayer == false)
        {
            if(isCameraSwitch == false)
            {
                int overlapCount = getMapCount(playerTransform.position);
                if(overlapCount == 2)
                {
                    if (inMap(lastPlayerPosition, nowMapIndex) && !inMap(lastPlayerPosition, lastMapIndex))
                    {
                        int temp = nowMapIndex;
                        nowMapIndex = lastMapIndex;
                        lastMapIndex = temp;
                    }
                    if (!inMap(lastPlayerPosition, nowMapIndex) && inMap(lastPlayerPosition, lastMapIndex))
                    {
                        float leftDis = abs(playerTransform.position.x - stageList[nowMapIndex][0]);
                        float rightDis = abs(playerTransform.position.x - stageList[nowMapIndex][1]);
                        float upDis = abs(playerTransform.position.y - stageList[nowMapIndex][2]);
                        float downDis = abs(playerTransform.position.y - stageList[nowMapIndex][3]);
                        leftUpDis = leftDis + upDis;
                        rightUpDis = rightDis + upDis;
                        leftDownDis = leftDis + downDis;
                        rightDownDis = rightDis + downDis;
                        if (leftUpDis <= rightUpDis && leftUpDis <= leftDownDis && leftUpDis <= rightDownDis)
                        {
                            destPosition.x = stageList[nowMapIndex][0] + halfCameraViewWidth;
                            destPosition.y = stageList[nowMapIndex][2] - halfCameraViewHeight;
                        }
                        else if (rightUpDis <= leftUpDis && rightUpDis <= rightDownDis && rightUpDis <= leftDownDis)
                        {
                            destPosition.x = stageList[nowMapIndex][1] - halfCameraViewWidth;
                            destPosition.y = stageList[nowMapIndex][2] - halfCameraViewHeight;
                        }
                        else if (rightDownDis <= leftUpDis && rightDownDis <= rightUpDis && rightDownDis <= leftDownDis)
                        {
                            destPosition.x = stageList[nowMapIndex][1] - halfCameraViewWidth;
                            destPosition.y = stageList[nowMapIndex][3] + halfCameraViewHeight;
                        }
                        else if (leftDownDis <= leftUpDis && leftDownDis <= rightUpDis && leftDownDis <= rightDownDis)
                        {
                            destPosition.x = stageList[nowMapIndex][0] + halfCameraViewWidth;
                            destPosition.y = stageList[nowMapIndex][3] + halfCameraViewHeight;
                        }
                        xPerStep = (destPosition.x - transform.position.x) / frameBetweenView;
                        yPerStep = (destPosition.y - transform.position.y) / frameBetweenView;
                        isCameraSwitch = true;
                    }
                }
                else if(overlapCount == 1)
                {
                    lastMapIndex = nowMapIndex;
                }
            }
            else
            {
                Vector3 tempPosition = transform.position;
                tempPosition.x += xPerStep;
                tempPosition.y += yPerStep;
                transform.position = tempPosition;
                if (abs(transform.position.x - destPosition.x) < eps && abs(transform.position.y - destPosition.y) < eps) isCameraSwitch = false;
            }
        }
        if (isCameraSwitch == true)
        {
            isCameraFollowPlayer = false;
        }
        else if (playerTransform.position.x - stageList[nowMapIndex][0] > halfCameraViewWidth && stageList[nowMapIndex][1] - playerTransform.position.x > halfCameraViewWidth)
        {
            isCameraFollowPlayer = true;
            moveType = 1;
        }
        else if (stageList[nowMapIndex][2] - playerTransform.position.y > halfCameraViewHeight && playerTransform.position.y - stageList[nowMapIndex][3] > halfCameraViewHeight)
        {
            isCameraFollowPlayer = true;
            moveType = 2;
        }
        else
            isCameraFollowPlayer = false;
    }

    void LateUpdate()
    {
        if(isCameraFollowPlayer)
        {
            Vector3 tempPosition = transform.position;
            if (moveType == 1) tempPosition.x = playerTransform.position.x;
            if (moveType == 2) tempPosition.y = playerTransform.position.y;
            transform.position = tempPosition;
        }
        lastPlayerPosition = playerTransform.position;
    }

    float abs(float x)
    {
        if (x >= 0) return x;
        else return -x;
    }

    int getMapCount(Vector3 nowPos)
    {
        int overlapCount = 0;
        for(int i = 0; i < stageCount; i++)
        {
            if (nowPos.x >= stageList[i][0] && nowPos.x <= stageList[i][1] && nowPos.y <= stageList[i][2] && nowPos.y >= stageList[i][3])
            {
                overlapCount++;
                if (overlapCount == 1) nowMapIndex = i;
                else if (overlapCount == 2) lastMapIndex = i;
            }
        }
        return overlapCount;
    }

    bool inMap(Vector3 nowPos, int mapIndex)
    {
        if (nowPos.x >= stageList[mapIndex][0] && nowPos.x <= stageList[mapIndex][1] && nowPos.y <= stageList[mapIndex][2] && nowPos.y >= stageList[mapIndex][3])
            return true;
        else return false;
    }
}