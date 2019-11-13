using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Trigger : MonoBehaviour
{
    public int Ani_Choose;
    bool HasEnterArea1;
    // Start is called before the first frame update
    void Start()
    {
        HasEnterArea1 = false;
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
            if (Ani_Choose == 0&&HasEnterArea1==false)
            {
                HasEnterArea1 = true;
                Fungus.Flowchart.BroadcastFungusMessage("Area2");
            }
            if (Ani_Choose == 1)
            {
                Fungus.Flowchart.BroadcastFungusMessage("Area3");
            }
        }

    }

}
