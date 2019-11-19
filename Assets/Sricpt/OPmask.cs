using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class OPmask : MonoBehaviour
{
    public float biggertime;
    public float smalltime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void bigmask()
    {
        transform.DOScale(new Vector3(50, 50, 1), biggertime);
    }
    void smallmask()
    {
        transform.DOScale(new Vector3(0, 0, 1), smalltime);
    }
}
