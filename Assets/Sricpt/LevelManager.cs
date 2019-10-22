using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform spawnPosition;
    public Transform PlayerTransform;
    public bool isDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PosReset()
    {
        PlayerTransform.position = spawnPosition.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerTransform.position.y < -10)
        {
            isDeath = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Deadly")
        {
            isDeath = true;
        }
        throw new NotImplementedException("Nothing");
    }
}
