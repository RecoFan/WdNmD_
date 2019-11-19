using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Trigger : MonoBehaviour
{
    public int Ani_Choose;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("1");
            if (Ani_Choose == 0)
            {
              
                Fungus.Flowchart.BroadcastFungusMessage("Area2");
            }
            if (Ani_Choose == 1)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area3");
            }
            if(Ani_Choose==2)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area4");
            }
            if (Ani_Choose == 3)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area5");
            }
            if(Ani_Choose==4)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area6");
            }
            if (Ani_Choose == 5)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area7");
            }
            if (Ani_Choose == 6)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area8");
            }
            if (Ani_Choose == 7)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area9");
            }
            if (Ani_Choose == 8)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area10");
            }
            if (Ani_Choose == 9)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area11");
            }
        }

    }

}
