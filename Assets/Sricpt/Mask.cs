using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ani.SetBool("InMask", true);
        Collider2D[] col;
        col = GetComponentsInChildren<Collider2D>();
        foreach(var child in col)
        {
            if (child != GetComponent<Collider2D>())
                child.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ani.SetBool("InMask", false);
        Collider2D[] col;
        col = GetComponentsInChildren<Collider2D>();
        foreach (var child in col)
        {
            if (child != GetComponent<Collider2D>())
                child.enabled = false;
        }
    }
}
