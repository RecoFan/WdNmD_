using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyThing : MonoBehaviour
{


    [Header("Moving")]
    [Space]
    public float perRadian = 0.03f;
    public float radius = 0.08f;
    float radian = 0;
    Vector3 oldPos;

    [Header("Circle")]
    [Space]
    public Transform aroundPoint;
    public float angularSpeed;
    public float aroundRadius;
    private float angled;

    [Header("Boolen")]
    public bool isCircle;

    // Start is called before the first frame update
    void Start()
    {
        if(isCircle)
        { 
            transform.position = new Vector3(aroundPoint.position.x*-1*aroundRadius, aroundPoint.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCircle)
        {
            oldPos = transform.position;
            radian += perRadian;
            float dy = Mathf.Cos(radian) * radius;
            transform.position = oldPos + new Vector3(0, dy, 0);
        }
        else
        {
            angled += (angularSpeed * Time.deltaTime) % 360;
            float posX = aroundRadius * Mathf.Sin(angled * Mathf.Deg2Rad);
            float posY = aroundRadius * Mathf.Cos(angled * Mathf.Deg2Rad);

            transform.position = new Vector3(posX, posY, 0) + aroundPoint.position;

        }
    }
}
