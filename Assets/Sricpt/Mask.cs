using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class Mask : MonoBehaviour
{
    // Animator ani;
    //PostProcessVolume m_Volume;
    // Bloom b;
    float radian = 0;
    Vector3 oldPos;


    [Space]
    [Header("ExitTime")]
    public float Static_Exit_Time = 2f;
    public float Exit_Time;
   

    [Space]
    [Header("Size")]
    public float old_x;
    public float old_y;
    public float old_z;
    public float new_x;
    public float new_y;
    public float new_z;


    [Space]
    [Header("Boolen")]
    public bool is_exit;
    public bool is_bigger;
    public bool is_float;
    public bool is_move;

    [Space]
    [Header("Time")]
    public float BiggerTime;
    public float SmallerTime;

    [Space]
    [Header("Float")]
    public float perRadian = 0.03f;
    public float radius = 0.08f;

    [Space]
    [Header("Moving")]
    public float newposition_x;
    public float newposition_y;
    public float oldposition_x;
    public float oldposition_y;

    public float running_time;

    bool isgoing;

    // Start is called before the first frame update
    void Start()
    {
        Exit_Time = Static_Exit_Time;

        if (is_move)
        {
            Sequence mySequence = DOTween.Sequence();
            Tweener moveto = transform.DOLocalMove(new Vector3(newposition_x, newposition_y, transform.localPosition.z), running_time);
            Tweener moveback = transform.DOLocalMove(new Vector3(oldposition_x, oldposition_y, transform.localPosition.z), running_time);
            mySequence.Append(moveto);
            mySequence.Append(moveback);
            mySequence.SetLoops(-1);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(is_float)
        {
            oldPos = transform.position;
            radian += perRadian;
            float dy = Mathf.Cos(radian) * radius;
            transform.position = oldPos + new Vector3(0, dy, 0);
        }

 



        if (is_exit)
        {
            Exit_Time -= Time.deltaTime;
            if(Exit_Time<=0)
            {
                transform.DOScale(new Vector3(old_x, old_y, old_z), SmallerTime);
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
       // ani.SetBool("InMask", true);

        /*
       b = ScriptableObject.CreateInstance<Bloom>();
       b.enabled.Override(true);
       b.intensity.Override(25f);
       m_Volume = PostProcessManager.instance.QuickVolume(11, 0f, b);
       */

            
            Sequence quence = DOTween.Sequence();
            quence.Append(transform.DOScale(new Vector3(new_x, new_y, new_z), BiggerTime));
            quence.Append(transform.DOScale(new Vector3(new_x-3, new_y - 3, new_z - 2), 0.05f));
  


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
        //  RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        is_exit = true;

    }

 
}
