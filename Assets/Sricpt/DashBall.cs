using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashBall : MonoBehaviour
{
    // Start is called before the first frame update

    Animator ani;
    public bool is_Touch;
    Movement move;
    void Start()
    {
        ani = GetComponent<Animator>();
        move = FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
       // ani.SetBool("Is_Touch", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!is_Touch)
        {
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            is_Touch = true;
            move.hasDashed = false;
        }
    }

}
