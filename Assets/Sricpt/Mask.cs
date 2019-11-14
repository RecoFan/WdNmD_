using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class Mask : MonoBehaviour
{
    // Animator ani;
    PostProcessVolume m_Volume;
    ChromaticAberration ca;
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
    public bool is_portal;


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

    [Space]
    [Header("Portals")]
    public GameObject target;
    public bool haveEnter;

    [Space] [Header("SFX")] 
    public AudioClip EnterPortal;
    public AudioClip OutPortal;
    private AudioSource _audioSource;
    
    bool isgoing;
    bool Iscreate;
    // Start is called before the first frame update
    void Start()
    {
        Exit_Time = Static_Exit_Time;
        _audioSource = GetComponent<AudioSource>();

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
        if (!is_portal)
        {
            if (is_exit)
            {
                Exit_Time -= Time.deltaTime;
                if (Exit_Time <= 0)
                {
                    transform.DOScale(new Vector3(old_x, old_y, old_z), SmallerTime);
                    // RuntimeUtilities.DestroyVolume(m_Volume, true, true);
                    DOVirtual.Float(0.30f, 0.15f, SmallerTime, ca.intensity.Override).OnComplete(DestroyVolume);

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


    }

    private void DestroyVolume()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        Iscreate = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Camera.main.transform.DOComplete();
        //Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        if (collision.gameObject.tag == "Player")
        {
            if (!is_portal)
            {
                is_exit = false;
                if (!Iscreate)
                {
                    ca = ScriptableObject.CreateInstance<ChromaticAberration>();
                    ca.enabled.Override(true);

                    DOVirtual.Float(0.15f, 0.30f, BiggerTime, ca.intensity.Override);

                    m_Volume = PostProcessManager.instance.QuickVolume(12, 0f, ca);
                    Iscreate = true;
                }
                Sequence quence = DOTween.Sequence();
                quence.Append(transform.DOScale(new Vector3(new_x, new_y, new_z), BiggerTime));
                quence.Append(transform.DOScale(new Vector3(new_x - 3, new_y - 3, new_z), 0.05f));
                Collider2D[] col;
                col = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
                foreach (var child in col)
                {
                    if (child != GetComponent<Collider2D>())
                    {
                        child.enabled = true;
                    }
                }
            }
            else if(!haveEnter)
            {
                _audioSource.PlayOneShot(EnterPortal,1);
                ca = ScriptableObject.CreateInstance<ChromaticAberration>();
                ca.enabled.Override(true);

                m_Volume = PostProcessManager.instance.QuickVolume(12, 0f, ca);
                Camera.main.transform.DOComplete();
                Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
                Sequence quence = DOTween.Sequence();
                Sequence quence2 = DOTween.Sequence();
                quence2.Append(DOVirtual.Float(0.15f, 0.80f, 0.15f, ca.intensity.Override));
                quence2.Append(DOVirtual.Float(0.80f, 0.15f, 0.1f, ca.intensity.Override).OnComplete(DestroyVolume));

                //   quence.Append(transform.DOScale(new Vector3(new_x, new_y, new_z), BiggerTime));
                //  quence.Append(transform.DOScale(new Vector3(new_x - 3, new_y - 3, new_z - 2), 0.05f));
                quence.Append(transform.DOScale(new Vector3(old_x - 5, old_x - 5, old_x - 5), SmallerTime));
                quence.Append(transform.DOScale(new Vector3(old_x - 3, old_x - 3, old_x - 3), 0.05f));
                quence.Append(transform.DOScale(new Vector3(old_x, old_y, old_z), SmallerTime));
                collision.gameObject.transform.position = target.transform.position;
               
                target.GetComponent<Mask>().haveEnter = true; 
            }
            else
            {
                _audioSource.PlayOneShot(OutPortal,1);
                Sequence quence = DOTween.Sequence();
                quence.Append(transform.DOScale(new Vector3(old_x - 5, old_x - 5, old_x - 5), SmallerTime));
                quence.Append(transform.DOScale(new Vector3(old_x - 3, old_x - 3, old_x - 3), 0.05f));
                quence.Append(transform.DOScale(new Vector3(old_x, old_y, old_z), SmallerTime));
            }
        }
        if (collision.gameObject.tag == "Deadly")
        {
            collision.isTrigger = true;
        }
        if(collision.gameObject.tag=="Bouncy2"|| collision.tag == "Ground2")
            collision.isTrigger = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
      
            if (collision.gameObject.tag == "Player")
            {
                if (!is_portal)
                {
                    Debug.Log("1");
                    is_exit = true;
                }
                else
                    haveEnter=false;
            }
    

        if (collision.tag == "Deadly")
        {
            collision.isTrigger = false;
        }
        if (collision.gameObject.tag == "Bouncy2"|| collision.tag == "Ground2")
            collision.isTrigger = true;
    }

 
}
