using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIsland : MonoBehaviour
{
    public float perRadian = 0.03f;
    public float radius = 0.08f;
    float radian = 0;
    Vector3 oldPos;
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
