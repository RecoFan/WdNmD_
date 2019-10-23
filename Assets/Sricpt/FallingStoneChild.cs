using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStoneChild : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject father;
    void Start()
    {
        father = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        father.GetComponent<FallingBrick>().is_rock = true;


    }
}
