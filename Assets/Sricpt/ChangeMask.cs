using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMask : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MaskTrue()
    {
        this.GetComponent<SpriteMask>().enabled = true;
    }
    void MaskFalse()
    {
        this.GetComponent<SpriteMask>().enabled = false;
    }
}
