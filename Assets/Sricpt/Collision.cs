using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask legdeLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;


    private Collider2D coll_onGround;
 //   private Collider2D coll_onWall;
    private Collider2D coll_onRightWall;
    private Collider2D coll_onLeftWall;

    //public bool onLedge;
    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coll_onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        coll_onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        coll_onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        if (coll_onGround != null)
        {
            if (!coll_onGround.isTrigger)
                onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        }
        else
            onGround = false;

     //   if (coll_onLeftWall != null || coll_onRightWall != null)
       // {
         //   if ((coll_onLeftWall != null && !coll_onLeftWall.isTrigger) || (coll_onRightWall != null && !coll_onRightWall.isTrigger))
           //     onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
             //       || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
       // }
       // else
         //   onWall = false;

        if (coll_onRightWall != null)
        {
            if (!coll_onRightWall.isTrigger)
                onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        }
        else
            onRightWall = false;

        if (coll_onLeftWall != null)
        {
            if (!coll_onLeftWall.isTrigger)
                onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        }
        else
            onLeftWall = false;

        if (onLeftWall || onRightWall)
            onWall = true;
        else
            onWall = false;

      //  onLedge = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, legdeLayer)
        //   || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, legdeLayer);


        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
