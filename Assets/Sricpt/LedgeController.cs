using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeController : MonoBehaviour
{
    private Animator anim;
    private bool is_Ledge;
    private Collision Side_judge;
    private Rigidbody2D rb;

    public float jumpforce = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Side_judge = GetComponent<Collision>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(is_Ledge)
        {
            if (Side_judge.onRightWall)
            {/*
                Vector3 newPosition = transform.position;
                newPosition.y += speed * Time.deltaTime;
                newPosition.x += speed * Time.deltaTime;
                transform.position = newPosition;*/
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity += Vector2.up * jumpforce;
            }
            else if (Side_judge.onLeftWall)
            {
                //Vector3 newPosition = transform.position;
                //newPosition.y += speed * Time.deltaTime;
                //newPosition.x += -speed * Time.deltaTime;
                //transform.position = newPosition;

            }
            
        }
    }
    public void Is_Ledge()
    {
        is_Ledge = true;
    }
    public void Is_Not_Ledge()
    {
        is_Ledge = false;
    }
}
