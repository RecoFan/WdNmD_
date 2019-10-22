using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject player;
    private bool isDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -10)
        {
            isDeath = true;
        }
        
        if (isDeath)
        {
            Destroy(player, 0.01f);
            Instantiate(player, spawnPosition.position, Quaternion.identity);
            isDeath = false;
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
