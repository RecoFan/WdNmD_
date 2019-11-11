using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMove : MonoBehaviour
{
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(moveH, moveV, 0)*Time.deltaTime*speed);
    }
}
