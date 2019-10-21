using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
  //  public LayerMask legdeLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;


  //  private Collider2D[] coll_onGround;
 //   private Collider2D coll_onWall;
   // private Collider2D[] coll_onRightWall;
   // private Collider2D[] coll_onLeftWall;

    //public bool onLedge;
    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
  //  public Vector2 bottomOffset, rightOffset, leftOffset;

    public Vector2 pointRight1, pointRight2, pointLeft1, pointLeft2, pointDown1, pointDown2;

    
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] coll_onGround = new Collider2D[10];
        Collider2D[] coll_onRightWall = new Collider2D[10];
        Collider2D[] coll_onLeftWall = new Collider2D[10];
        int ground_count = Physics2D.OverlapAreaNonAlloc((Vector2)transform.position + pointDown1, (Vector2)transform.position + pointDown2, coll_onGround,groundLayer);
       // coll_onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        int right_count = Physics2D.OverlapAreaNonAlloc((Vector2)transform.position + pointRight1, (Vector2)transform.position + pointRight2, coll_onRightWall,groundLayer);
        Debug.Log(right_count);
       int left_count = Physics2D.OverlapAreaNonAlloc((Vector2)transform.position + pointLeft1, (Vector2)transform.position + pointLeft2, coll_onLeftWall,groundLayer);

        if (ground_count != 0)
        {
            for(int i=0;i<ground_count;i++)
            {
                if (!coll_onGround[i].isTrigger)
                {
                    //   onGround = Physics2D.OverlapArea((Vector2)transform.position + pointDown1, (Vector2)transform.position + pointDown2, groundLayer);
                    onGround = true;
                    break;
                }
                else
                    onGround = false;
            }
        }
        else
            onGround = false;
        /*
       if (coll_onLeftWall != null || coll_onRightWall != null)
       {
           if ((coll_onLeftWall != null && !coll_onLeftWall.isTrigger) || (coll_onRightWall != null && !coll_onRightWall.isTrigger))
               onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
                  || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
       }
        else
           onWall = false;
         */
         
        if (right_count!= 0)
        {
            for (int i = 0; i < right_count;i++)
            {
                if (!coll_onRightWall[i].isTrigger)
                {
                    // Debug.Log("1");
                    //onRightWall = Physics2D.OverlapArea((Vector2)transform.position + pointRight1, (Vector2)transform.position + pointRight2, groundLayer);
                    onRightWall = true;
                    break;
                }
                else
                    onRightWall = false;
            }
        }
        else
            onRightWall = false;
            
            
        if (left_count != 0)
        {
            for (int i = 0; i < left_count; i++)
            {
                if (!coll_onLeftWall[i].isTrigger)
                {
                    onLeftWall = true;
                    break;
                    // onLeftWall = Physics2D.OverlapArea((Vector2)transform.position + pointLeft1, (Vector2)transform.position + pointLeft2, groundLayer);
                }
                else
                    onLeftWall = false;
            }
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

      //  var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + pointRight1, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + pointRight2, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + pointLeft1, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + pointLeft2,collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + pointDown1, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + pointDown2, collisionRadius);
  
    }
}
