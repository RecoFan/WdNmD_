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
    public ParticleSystem idie;
    public float recoverTime;
    public float static_recoverTime = 5f;
    float radian = 0;
    Vector3 oldPos;
    public float perRadian = 0.03f;
    public float radius = 0.08f;
    void Start()
    {
        oldPos = transform.position;
        idie.Play();
        recoverTime = static_recoverTime;
        move = FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        // ani.SetBool("Is_Touch", true);
        oldPos = transform.position;
        radian += perRadian;
        float dy = Mathf.Cos(radian) * radius;
        transform.position = oldPos + new Vector3(0, dy, 0);
        if (is_Touch)
        {
            recoverTime -= Time.deltaTime;
            if(recoverTime <=0)
            {
                idie.Play();
                recoverTime = static_recoverTime;
                is_Touch = false;
                GetComponent<SpriteRenderer>().enabled = true;
                transform.localScale = new Vector3(0, 0, 0);
                transform.DOScale(new Vector3(2.572046f, 2.572046f, 2.572046f), 0.2f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Mask")
        {
            if (!is_Touch)
            {
                disappear.Play();
                idie.Stop();
                Camera.main.transform.DOComplete();
                Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
                GetComponent<SpriteRenderer>().enabled = false;
                is_Touch = true;
                move.hasDashed = false;
            }
        }
    }

}
