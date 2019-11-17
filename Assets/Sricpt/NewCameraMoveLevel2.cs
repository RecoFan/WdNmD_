using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NewCameraMoveLevel2 : MonoBehaviour
{
    public float[][] cameraLocationList;
    public float[][] stageList;
    private int stageCount = 12;

    [Space] [Header("Player")] 
    public Transform playerTransform;
    private Vector3 lastPlayerPosition;

    [Space]
    [Header("Move Speed between Stage")]
    public int frameBetweenView = 10;
    public int moveCount = 0;
    //----------Destination------------------
    private float eps = 0.1f;
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
    public int lastCameraMapIndex;
    public int cameraNowMapIndex;
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
        playerTransform = GameObject.FindWithTag("Player").transform;
        halfCameraViewHeight = GetComponent<Camera>().orthographicSize;
        halfCameraViewWidth = halfCameraViewHeight * UnityEngine.Screen.width / UnityEngine.Screen.height;

        cameraLocationList = new float[12][];
        cameraLocationList[0] = new float[] {42.6f, 9.5f };
        cameraLocationList[1] = new float[] { 81.8f, 25.8f };
        cameraLocationList[2] = new float[] { 112.65f, 48.18f};
        cameraLocationList[3] = new float[] { 152.04f, 48.18f };
        cameraLocationList[4] = new float[] { 185.4f, 54.6f };
        cameraLocationList[5] = new float[] { 214.2f, 57f };
        cameraLocationList[6] = new float[] { 247.3f, 71.7f };
        cameraLocationList[7] = new float[] { 254.5f, 88.4f };
        cameraLocationList[8] = new float[] {287.6f, 96.1f };
        cameraLocationList[9] = new float[] { 327.6f, 102.4f };
        cameraLocationList[10] = new float[] { 356.4f, 124.9f };
        cameraLocationList[11] = new float[] { 374.9f, 101.3f };

        stageList = new float[12][];
        stageList[0] = new float[] { 0, 0, 0, 0}; //xLeft,xRight,yUp,yDown
        stageList[1] = new float[] { 0, 0, 0, 0};
        stageList[2] = new float[] { 0, 0, 0, 0 };
        stageList[3] = new float[] { 0, 0, 0, 0 };
        stageList[4] = new float[] { 0, 0, 0, 0 };
        stageList[5] = new float[] { 0, 0, 0, 0 };
        stageList[6] = new float[] { 0, 0, 0, 0 };
        stageList[7] = new float[] { 0, 0, 0, 0 };
        stageList[8] = new float[] { 0, 0, 0, 0 };
        stageList[9] = new float[] { 0, 0, 0, 0 };
        stageList[10] = new float[] { 0, 0, 0, 0 };
        stageList[11] = new float[] { 0, 0, 0, 0 };

        for (int i = 0; i < 12; i++)
        {
            stageList[i][0] = cameraLocationList[i][0] - halfCameraViewWidth;
            stageList[i][1] = cameraLocationList[i][0] + halfCameraViewWidth;
            stageList[i][2] = cameraLocationList[i][1] + halfCameraViewHeight;
            stageList[i][3] = cameraLocationList[i][1] - halfCameraViewHeight;
        }
        //stageList[2][0] += 4.9f;
        //stageList[2][1] += 7f;

        isCameraFollowPlayer = true;
        isCameraSwitch = false;
        //Vector3 tempVector = playerTransform.position;
        //tempVector.x = transform.position.x;
        //playerTransform.position = tempVector;

        lastPlayerPosition = playerTransform.position;

        int overlapCount = getMapCount(playerTransform.position);
        if(overlapCount == 1) //must
        {
            lastMapIndex = nowMapIndex;
            cameraNowMapIndex = lastCameraMapIndex = nowMapIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.x + "," + transform.position.y);
        if (isCameraFollowPlayer == false)
        {
            if(isCameraSwitch == false)
            {
                int tempNowMapIndex = nowMapIndex;
                int overlapCount = getMapCount(playerTransform.position);
                if (overlapCount == 2)
                {
                    /*if (inMap(lastPlayerPosition, nowMapIndex) && !inMap(lastPlayerPosition, lastMapIndex))
                    {
                        int temp = nowMapIndex;
                        nowMapIndex = lastMapIndex;
                        lastMapIndex = temp;
                    }*/
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
                        //--------------camera location------------------
                        lastCameraMapIndex = getMapIndex(transform.position);
                        cameraNowMapIndex = getMapIndex(destPosition);
                        //--------------------------------------------------
                        moveCount = 0;
                        isCameraSwitch = true;


                        destPosition.z = -10f;
                        transform.DOMove(destPosition, 0.2f);
                    }
                }
                else if(overlapCount == 1)
                {
                    if (!inMap(playerTransform.position, cameraNowMapIndex))
                    {
                       
                        cameraNowMapIndex = getMapIndex(playerTransform.position);
                        lastCameraMapIndex = cameraNowMapIndex;

                        float leftDis = abs(playerTransform.position.x - stageList[cameraNowMapIndex][0]);
                        float rightDis = abs(playerTransform.position.x - stageList[cameraNowMapIndex][1]);
                        float upDis = abs(playerTransform.position.y - stageList[cameraNowMapIndex][2]);
                        float downDis = abs(playerTransform.position.y - stageList[cameraNowMapIndex][3]);
                        leftUpDis = leftDis + upDis;
                        rightUpDis = rightDis + upDis;
                        leftDownDis = leftDis + downDis;
                        rightDownDis = rightDis + downDis;
                        if (leftUpDis <= rightUpDis && leftUpDis <= leftDownDis && leftUpDis <= rightDownDis)
                        {
                            destPosition.x = stageList[cameraNowMapIndex][0] + halfCameraViewWidth;
                            destPosition.y = stageList[cameraNowMapIndex][2] - halfCameraViewHeight;
                        }
                        else if (rightUpDis <= leftUpDis && rightUpDis <= rightDownDis && rightUpDis <= leftDownDis)
                        {
                            destPosition.x = stageList[cameraNowMapIndex][1] - halfCameraViewWidth;
                            destPosition.y = stageList[cameraNowMapIndex][2] - halfCameraViewHeight;
                        }
                        else if (rightDownDis <= leftUpDis && rightDownDis <= rightUpDis && rightDownDis <= leftDownDis)
                        {
                            destPosition.x = stageList[cameraNowMapIndex][1] - halfCameraViewWidth;
                            destPosition.y = stageList[cameraNowMapIndex][3] + halfCameraViewHeight;
                        }
                        else if (leftDownDis <= leftUpDis && leftDownDis <= rightUpDis && leftDownDis <= rightDownDis)
                        {
                            destPosition.x = stageList[cameraNowMapIndex][0] + halfCameraViewWidth;
                            destPosition.y = stageList[cameraNowMapIndex][3] + halfCameraViewHeight;
                        }
                        xPerStep = (destPosition.x - transform.position.x) / frameBetweenView;
                        yPerStep = (destPosition.y - transform.position.y) / frameBetweenView;
                        moveCount = 0;
                        isCameraSwitch = true;

                        destPosition.z = -10f;
                        transform.DOMove(destPosition, 0.2f);

                        nowMapIndex = cameraNowMapIndex;
                    }
                    lastMapIndex = nowMapIndex;
                }
            }
            else
            {
                /*Vector3 tempPosition = transform.position;
                tempPosition.x += xPerStep;
                tempPosition.y += yPerStep;
                transform.position = tempPosition;*/
                if (abs(transform.position.x - destPosition.x) < eps && abs(transform.position.y - destPosition.y) < eps) isCameraSwitch = false;
                /*moveCount++;
                if (moveCount >= frameBetweenView)
                {
                    isCameraSwitch = false;
                    moveCount = 0;
                }*/
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

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
   //     Debug.Log(playerTransform);
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

    int getMapIndex(Vector3 nowPos)
    {
        for (int i = 0; i < stageCount; i++)
        {
            if (nowPos.x >= stageList[i][0] && nowPos.x <= stageList[i][1] && nowPos.y <= stageList[i][2] && nowPos.y >= stageList[i][3])
            {
                return i;
            }
        }
        return -1;
    }

    int getMapCount(Vector3 nowPos)
    {
        int overlapCount = 0;
        for(int i = 0; i < stageCount; i++)
        {
            if (nowPos.x >= stageList[i][0] && nowPos.x <= stageList[i][1] && nowPos.y <= stageList[i][2] && nowPos.y >= stageList[i][3])
            {
                overlapCount++;
            }
        }
        if (overlapCount == 2)
        {
            for (int i = 0; i < stageCount; i++)
            {
                if (nowPos.x >= stageList[i][0] && nowPos.x <= stageList[i][1] && nowPos.y <= stageList[i][2] && nowPos.y >= stageList[i][3])
                {
                    if (nowMapIndex != i)
                    {
                        lastMapIndex = nowMapIndex;
                        nowMapIndex = i;
                        break;
                    }
                }
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
