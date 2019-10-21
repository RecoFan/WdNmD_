using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashBall : MonoBehaviour
{
    // Start is called before the first frame update

    public bool is_Touch;
    Movement move;
    public ParticleSystem disappear;
    public float recoverTime;
    public float static_recoverTime = 5f;
    void Start()
    {
        recoverTime = static_recoverTime;
        move = FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
       // ani.SetBool("Is_Touch", true);
       if(is_Touch)
        {
            recoverTime -= Time.deltaTime;
            if(recoverTime <=0)
            {
                recoverTime = static_recoverTime;
                is_Touch = false;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!is_Touch)
        {
            disappear.Play();
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            GetComponent<SpriteRenderer>().enabled = false;
            is_Touch = true;
            move.hasDashed = false;
        }
    }

}
