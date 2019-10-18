using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBrick : MonoBehaviour
{
    public float falling_time = 2f;
    public float time_to_back = 3f;
    public bool is_back;
    public bool is_falling;
    private Collider2D cd;
    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_falling)
        {
            falling_time -= Time.deltaTime;
            if (falling_time <= 0)
            {
                is_falling = false;
                cd.enabled = false;
                falling_time = 2f;
                is_back = true;
            }
        }
        if(is_back)
        {
            time_to_back -= Time.deltaTime;
            if(time_to_back<=0)
            {
                cd.enabled = true;
                time_to_back = 3f;
                is_back = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        is_falling = true;

    }

}
