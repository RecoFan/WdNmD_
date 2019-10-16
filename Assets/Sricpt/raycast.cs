using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class raycast : MonoBehaviour
{
    private Collision coll;
    private RaycastHit2D hitl, hitr, hit1;
    private Vector2 Originpos, ray_drc;
    private LayerMask mask;
    
    public float chestheight;
    public float maxraylength;
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
                ray_drc = new Vector2(maxraylength, jumpable).normalized;
                hit1 = Physics2D.Raycast(Originpos, ray_drc, 2, mask);
                if (hit1.collider == null)
                {
                    OnLedge = true;
                }
                else OnLedge = false;
            }
            else OnLedge = false;

        }
        else if (coll.onLeftWall)
        {
            hitl = Physics2D.Raycast(Originpos, -transform.right, maxraylength, mask);
            if (hitl.collider != null)
            {
                ray_drc = new Vector2(-maxraylength, jumpable).normalized;
                hit1 = Physics2D.Raycast(Originpos, ray_drc, 2, mask);
                if (hit1.collider == null)
                {
                    OnLedge = true;
                }
                else OnLedge = false;
            }
            else OnLedge = false;

        }
        else OnLedge = false;
    }
}