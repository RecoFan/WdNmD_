using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyThing : MonoBehaviour
{
    float radian = 0;
    Vector3 oldPos;
    public float perRadian = 0.03f;
    public float radius = 0.08f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        oldPos = transform.position;
        radian += perRadian;
        float dy = Mathf.Cos(radian) * radius;
        transform.position = oldPos + new Vector3(0, dy, 0);
    }
}
