using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class raycast : MonoBehaviour
{
    private Collision coll;
    private RaycastHit2D hit1;
    private Vector2 Originpos, ray_drcL, ray_drcR;
    private LayerMask mask;
    
    public float chestheight;
    public float maxraylength;
    public float jumpable;
    public bool OnLedge;
    
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        mask = LayerMask.GetMask("Test");
        ray_drcR = new Vector2(maxraylength, jumpable).normalized;
        ray_drcL = new Vector2(-maxraylength, jumpable).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Originpos = transform.position;
        Originpos.y += chestheight;
        
        
        if (coll.onRightWall)
        {
            
            hit1 = Physics2D.Raycast(Originpos, ray_drcR, 2, mask);
            if (hit1.collider == null)
            {
                OnLedge = true;
            }
            else OnLedge = false;

        }
        else if (coll.onLeftWall)
        {
            hit1 = Physics2D.Raycast(Originpos, ray_drcL, 2, mask);
            if (hit1.collider == null)
            {
                OnLedge = true;
            }
            else OnLedge = false;
            
        }
        else OnLedge = false;
    }
}