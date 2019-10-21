using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class Mask : MonoBehaviour
{
    Animator ani;
    PostProcessVolume m_Volume;
    Bloom b;
    public float Static_Exit_Time = 2f;
    public float Exit_Time;
    bool is_exit;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        Exit_Time = Static_Exit_Time;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_exit)
        {
            Exit_Time -= Time.deltaTime;
            if(Exit_Time<=0)
            {
                ani.SetBool("InMask", false);
                is_exit = false;
                Exit_Time = Static_Exit_Time;
                Collider2D[] col;
                col = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
                foreach (var child in col)
                {
                    if (child != GetComponent<Collider2D>())
                    {
                        child.enabled = false;
                    }
                }
            }
        }
        else
            Exit_Time = Static_Exit_Time;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Camera.main.transform.DOComplete();
        //Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        is_exit = false;
        ani.SetBool("InMask", true);
        b = ScriptableObject.CreateInstance<Bloom>();
        b.enabled.Override(true);
        b.intensity.Override(25f);
        m_Volume = PostProcessManager.instance.QuickVolume(11, 0f, b);
        Collider2D[] col;
        col = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
        foreach(var child in col)
        {
            if (child != GetComponent<Collider2D>())
            {
             

                child.enabled = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        is_exit = true;



    }
}
