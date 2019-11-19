using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FadeOP : MonoBehaviour
{
    public float fade_out;
    public float fade_in;
    public bool iscanvas;
    // Start is called before the first frame update
    private void Awake()
    {
        if(!iscanvas)
        GetComponent<SpriteRenderer>().DOFade(0, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fadein()
    {
        GetComponent<SpriteRenderer>().DOFade(1, fade_in);
    }
    void fadeout()
    {
        Debug.Log("1");
        GetComponent<SpriteRenderer>().DOFade(0, fade_out);
    }
}
