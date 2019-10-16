using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class raycast : MonoBehaviour
{
    private Collision coll;
    private RaycastHit2D hitl, hitr, hit1;
    private Vector2 Originpos, rayStart;
    private LayerMask mask;
    
    public float chestheight;
    public float maxraylength;
    public float maxrayheight;
    public float jumpable;
    public bool OnLedge;
    
    private Rigidbody2D example;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        mask = LayerMask.GetMask("Test");
    }

    // Update is called once per frame
    void Update()
    {
        Originpos = transform.position;
        Originpos.y += chestheight;
        
        
        if (coll.onRightWall)
        {
            hitr = Physics2D.Raycast(Originpos, transform.right, maxraylength, mask);
            if (hitr.collider != null)
            {
                rayStart = hitr.point + (Vector2)transform.right * 0.1f;
                rayStart.y += 10.0f;
                hit1 = Physics2D.Raycast(rayStart, Vector2.down, maxrayheight, mask);
            }
            if (hit1.collider != null)
            {
                if (Mathf.Abs(transform.position.y - hit1.point.y) < jumpable)
                    OnLedge = true;
            }
        }
        else if (coll.onLeftWall)
        {
            hitl = Physics2D.Raycast(Originpos, -transform.right, maxraylength, mask);
            if (hitl.collider != null)
            {
                rayStart = hitl.point - (Vector2)transform.right * 0.1f;
                rayStart.y += 10.0f;
                hit1 = Physics2D.Raycast(rayStart, Vector2.down, maxrayheight, mask);
            }
            if (hit1.collider != null)
            {
                if (Mathf.Abs(transform.position.y - hit1.point.y) < jumpable)
                    OnLedge = true;
            }
        }
        else OnLedge = false;
    }
}