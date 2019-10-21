using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class Mask : MonoBehaviour
{
    Animator ani;
    PostProcessVolume m_Volume;
    Bloom b;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ani.SetBool("InMask", true);
        Collider2D[] col;
       
        col = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
        foreach(var child in col)
        {
            if (child != GetComponent<Collider2D>())
            {
                b = ScriptableObject.CreateInstance<Bloom>();
                b.enabled.Override(true);
                b.intensity.Override(25f);
                m_Volume = PostProcessManager.instance.QuickVolume(11, 0f, b);

                child.enabled = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ani.SetBool("InMask", false);
        Collider2D[] col;
        col = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
        foreach (var child in col)
        {
            if (child != GetComponent<Collider2D>())
            {
                RuntimeUtilities.DestroyVolume(m_Volume, true, true);
                child.enabled = false;
            }
        }
    }
}
