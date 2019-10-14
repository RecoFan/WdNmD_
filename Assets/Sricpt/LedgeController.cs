using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeController : MonoBehaviour
{
    private Animator anim;
    private bool is_Ledge;

    public float speed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(is_Ledge)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += speed * Time.deltaTime;
            newPosition.x += speed * Time.deltaTime;
            transform.position = newPosition;

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
