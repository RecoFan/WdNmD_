using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArea : MonoBehaviour
{
    public bool Up_Or_Down;
    public int Direction;   //1 right 2 left 3 up 4 down
    public float Up_Speed = 12f;
    public float Down_Speed = -12f;
    Movement move;
    // Start is called before the first frame update
    void Start()
    {
        move = FindObjectOfType<Movement>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Up_Or_Down)
        {
            if (Direction == 1)
                move.Ho_Speed = Up_Speed;
            if (Direction == 2)
                move.Ho_Speed = Down_Speed;
            if (Direction == 3)
                move.Ve_Speed = Up_Speed;
            if (Direction == 4)
                move.Ve_Speed = Down_Speed;
            Debug.Log("1");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        move.Ho_Speed = 0;
        move.Ve_Speed = 0;
    }
}
