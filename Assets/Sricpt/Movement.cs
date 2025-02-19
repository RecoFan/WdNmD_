﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collision coll;
    private AnimationScript anim;
    private Color raw_Color;
    private raycast Ledge_judge;



    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public float enduranceBar = 100;
    public float walljumpDecrease = 10;
    public float wallGrabDecrease = 5;
    public float blink_Time_1 = 0f;
    public float blink_Time_2 = 0f;



    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    public bool Str_WallJumped;
    public bool hasDashed;
    public bool isDeath;
    public bool isBounce;
    public bool New_Can_Move;

    [Space]

    private bool groundTouch;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    public GameObject dashParticle_2;
    public GhostTrail ghost;

    [Space]
    [Header("Grace")]
    public int graceJumpTime = 10;
    public int graceTimer;

    [Space]
    [Header("Buffer")]
    public float jumpBufferTimer;
    public float jumpBuffer = 5f;

    [Space] 
    [Header("SFX")]
    public AudioClip SFX_Spring;
    public AudioClip SFX_Dash;
    public AudioClip SFX_Walk;
    public AudioClip SFX_Walk2;
    public AudioClip SFX_Jump;
    public AudioClip SFX_Jump2;
    public AudioClip SFX_JumpA;
    public AudioClip SFX_Climb;
    public AudioClip SFX_WallJump;
    private AudioSource[] _audioSource;
    private bool _SFX_Pernament = false;
    private bool _SFX_GroundTouchFirst = true;
     
    [Space]
    [Header("Ledge")]
    public float Ledge_Timer = 0.2f;
    public int Ledge_Is = 0;
    public bool HasLedged;
    public float HasLedged_Time = 0.1f;
    public float LedgeSpeed = 10f;

    [Space]
    [Header("ChangeSpeed")]
    public float Ho_Speed;
    public float Ve_Speed;
    public float Bo_Speed;

    [Space]
    [Header("DashState")]
    public float DargNum;
    public float TimeForDarg;
    public float DashTime;
    public float StartDashTime;
    public Vector2 savevelocity;
    //  float t1;
    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
       // Screen.fullScreen = false;
        New_Can_Move= LevelManager.instance.Move_State;
        _audioSource = GetComponents<AudioSource>();
        graceTimer = graceJumpTime;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        rb.freezeRotation= true;
        anim = GetComponentInChildren<AnimationScript>();
        raw_Color = GetComponent<SpriteRenderer>().color;
        Ledge_judge = GetComponent<raycast>();
        DashTime = StartDashTime;
 
    }

    // Update is called once per frame
    void Update()
    {

        if (New_Can_Move)
        {
            if (Ledge_Is != 0)
            {
                Ledge_Timer -= Time.deltaTime;
                if (Ledge_Timer <= 0)
                {
                    if (Ledge_Is > 0)
                        rb.velocity += Vector2.right * LedgeSpeed;
                    else if (Ledge_Is < 0)
                        rb.velocity += Vector2.left * LedgeSpeed;
                    HasLedged = true;
                    Str_WallJumped = false;
                    Ledge_Is = 0;
                }
            }
            else
            {
                Ledge_Timer = 0.2f;
            }

            if (HasLedged)
            {

                HasLedged_Time -= Time.deltaTime;
                if (HasLedged_Time > 0 && HasLedged_Time <= 0.05f)
                {
                    rb.velocity = Vector2.zero;
                }
                if (HasLedged_Time <= 0)
                {
                    HasLedged = false;
                }

            }
            else
            {
                HasLedged_Time = 0.15f;
            }
            if (!HasLedged && Ledge_Is == 0)
            {
                float x = 0;
                float y = 0;

                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");



                float x_t = x;
                float y_t = y;

                if (Input.GetButton("Horizontal"))
                {
                    if (Mathf.Abs(x) >= 0.5)
                    {
                        if (x > 0)
                            x_t = 1;
                        else
                            x_t = -1;
                    }
                    else
                        x_t = (x_t * 2) % 1;
                    if (Mathf.Abs(y) >= 0.5)
                    {
                        if (y > 0)
                            y_t = 1;
                        else
                            y_t = -1;
                    }
                    else
                        y_t = (y_t * 2) % 1;
                }
                else
                {

                    x_t = x_t / 2.0f;
                    y_t = y_t / 2.0f;
                }

                float xRaw = Input.GetAxisRaw("Horizontal");
                float yRaw = Input.GetAxisRaw("Vertical");
                dir = new Vector2(x_t, y_t);

                Walk(dir);
                anim.SetHorizontalMovement(x, y, rb.velocity.y);




                if (coll.onGround)
                {
                    enduranceBar = 100;
                    graceTimer = graceJumpTime;
                }
                else
                    graceTimer--;

                if (Str_WallJumped && rb.velocity.y <= 0)
                    Str_WallJumped = false;

                if (coll.onWall && Input.GetButton("Fire3") && canMove && !Str_WallJumped && !isDashing)
                {
                    if (side != coll.wallSide)
                    {
                        anim.Flip(side * -1);
                    }
                    if (enduranceBar <= 0)
                    {
                        wallGrab = false;
                        wallSlide = true;
                        WallSlide();
                    }
                    if (enduranceBar > 0)
                    {
                        wallGrab = true;
                        wallSlide = false;
                    }
                }

                if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
                {
                    wallGrab = false;
                    wallSlide = false;
                }



                if (coll.onGround && !isDashing)
                {
                    // ghost.makeGhost = false;
                    wallJumped = false;
                    GetComponent<BetterJumping>().enabled = true;
                }

                if (wallGrab && !isDashing)
                {
                    rb.gravityScale = 0;
                    if (x > .2f || x < -.2f)
                        rb.velocity = new Vector2(rb.velocity.x, 0);

                    float speedModifier = y > 0 ? .5f : 1;

                    rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
                    enduranceBar -= wallGrabDecrease * Time.deltaTime;
                }
                else if (rb.velocity.y > -20)
                {

                    rb.gravityScale = 3;
                }
                else
                    rb.gravityScale = 0;

                if (coll.onWall && !coll.onGround && !isDashing)
                {
                    if (x != 0 && !wallGrab)
                    {
                        wallSlide = true;
                        WallSlide();
                    }
                }

                if (!coll.onWall || coll.onGround)
                    wallSlide = false;


                if (Input.GetKeyDown("c"))
                    jumpBufferTimer = jumpBuffer;

                if (jumpBufferTimer > 0)
                {

                    anim.SetTrigger("jump");
                    if (coll.onGround || graceTimer > 0)
                    {
                        Jump(Vector2.up, false);
                        graceTimer = 0;
                        jumpBufferTimer = 0;
                    }

                    if (coll.onWall && !coll.onGround && !wallGrab)
                    {
                        WallJump();
                        enduranceBar -= walljumpDecrease;
                        jumpBufferTimer = 0;
                    }

                    if (coll.onWall && !coll.onGround && wallGrab &&
                        (Input.GetKey("right") || Input.GetKey("left")))
                    {
                        WallJump();
                        enduranceBar -= walljumpDecrease;
                        jumpBufferTimer = 0;
                    }

                    if (coll.onWall && !coll.onGround && wallGrab &&
                        !(Input.GetKey("right") || Input.GetKey("left")))
                    {
                        wallGrab = false;
                        Str_WallJumped = true;
                        Jump(Vector2.up, true);
                        enduranceBar -= walljumpDecrease;
                        jumpBufferTimer = 0;
                    }
                    jumpBufferTimer--;
                }

                if (isBounce)
                {
                    _audioSource[0].PlayOneShot(SFX_Spring, 1);
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.velocity += Vector2.up * Bo_Speed;
                    anim.SetTrigger("jump");
                    isBounce = false;
                    _SFX_Pernament = true;
                }

                if (Ledge_judge.OnLedge && wallGrab && Input.GetKey("up"))
                {

                    if (coll.onRightWall)
                    {
                        side = 1;
                        anim.Flip(side);
                        wallGrab = false;
                        Str_WallJumped = true;
                        Jump(Vector2.up, true);
                        anim.SetTrigger("jump");
                        Ledge_Is = 1;
                        jumpBufferTimer = 0;
                    }
                    else if (coll.onLeftWall)
                    {
                        side = -1;
                        anim.Flip(side);
                        wallGrab = false;
                        Str_WallJumped = true;
                        Jump(Vector2.up, true);
                        anim.SetTrigger("jump");
                        Ledge_Is = -1;
                        jumpBufferTimer = 0;
                    }
                    return;

                }

                WallParticle(y);

                Blink_To_Red();


                if (Input.GetKeyDown("x") && !hasDashed && enduranceBar > 0)
                {
                    if (xRaw != 0 || yRaw != 0)
                    {
                        Dash(xRaw, yRaw);
                    }
                }

                if (coll.onGround && !groundTouch)
                {
                    GroundTouch();
                    groundTouch = true;
                }

                if (!coll.onGround && groundTouch)
                {
                    groundTouch = false;
                }

                if (wallGrab || wallSlide || !canMove)
                    return;
                if (!isDashing)
                {
                    if (wallGrab && !isDashing)
                    {
                    }
                    else if (rb.velocity.y > -20)
                    {
                        rb.gravityScale = 3;
                    }
                    else
                        rb.gravityScale = 0;

                    // rb.velocity = savevelocity;
                    //rb.gravityScale = 3;
                    GetComponent<BetterJumping>().enabled = true;
                    //wallJumped = false;
                    isDashing = false;
                    ghost.makeGhost = false;
                    DashTime = StartDashTime;
                }
                if (x > 0)
                {
                    side = 1;
                    anim.Flip(side);
                }

                if (x < 0)
                {
                    side = -1;
                    anim.Flip(side);
                }

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Deadly")
        {
            isDeath = true;
        }
        else if (other.gameObject.tag == "Bouncy"|| other.gameObject.tag == "Bouncy2")
        {
            isBounce = true;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        //Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Deadly")
        {
            isDeath = true;
        }
        /*else if (other.gameObject.tag == "Bouncy"|| other.gameObject.tag == "Bouncy2")
        {
            isBounce = true;
        }*/
    }

    void GroundTouch()
    {
        if (_SFX_GroundTouchFirst)
            _SFX_GroundTouchFirst = false;
        else Play_SFX_Jump_Over();
        rb.gravityScale = 3;
        hasDashed = false;
        isDashing = false;
        ghost.makeGhost = false;
        side = anim.sr.flipX ? -1 : 1;
        jumpParticle.Play();
    }
    private void WallSlide()
    {
        if (coll.wallSide != side)
            anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;

        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new Vector2(push, -slideSpeed);
        slideParticle.Play();
    }

    private void Dash(float x, float y)
    {
        _audioSource[0].PlayOneShot(SFX_Dash,1);
        _SFX_Pernament = true;
        StartCoroutine(GroundDash());
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.15f,0.5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;
        wallGrab = false;
        anim.SetTrigger("dash");

        savevelocity = rb.velocity;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);
        rb.velocity += dir.normalized * dashSpeed;

        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;
        ghost.makeGhost = true;
        dashParticle.Play();
        Instantiate(dashParticle_2, transform.position, Quaternion.identity);
        //StartCoroutine(DashWait());

    }


    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
           // t1 = 0;
            rb.velocity = (new Vector2(dir.x * speed+Ho_Speed, rb.velocity.y+Ve_Speed));
        }
        else if(!isDashing)
        {
           // Debug.Log("1234");
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)),wallJumpLerp*Time.deltaTime);
        }
        if(isDashing)
        {
            if(DashTime>0)
            {
                DashTime -= Time.deltaTime;
            }
        
            else 
            {
                
                if (rb.velocity.y > 10)
                {
                    rb.velocity = new Vector2(0, 5);
                    anim.SetTrigger("jump");
                }
                
                else
                    rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 3;
                
               // rb.velocity = savevelocity;
                //rb.gravityScale = 3;
                GetComponent<BetterJumping>().enabled = true;
                wallJumped = false;
                isDashing = false;
                ghost.makeGhost = false;
                DashTime = StartDashTime;
            }
        }


    }
    private void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
        particle.Play();
    }
    private void WallJump()
    {
        
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f),true);

        wallJumped = true;
    }
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }



    IEnumerator DashWait()
    {
        //   FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(DargNum, 0, TimeForDarg, RigidbodyDrag);
        
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;
    
        ghost.makeGhost = true;
        dashParticle.Play();
        //  dashParticle_2.Play();


        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 3;
       
        //DOVirtual.Float(rb.velocity.x, dir.x * speed, 0.05f, ChangeVelocityX);
        //DOVirtual.Float(rb.velocity.y, dir.y * speed, 0.051f, ChangeVelocityY);
        GetComponent<BetterJumping>().enabled = true;


        wallJumped = false;
        isDashing = false;
        ghost.makeGhost = false;
    }



    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }
    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    void Blink_To_Red()
    {
        if (enduranceBar <= 50 && enduranceBar >= 25)
        {

            blink_Time_1 += Time.deltaTime;
            if (blink_Time_1 % 0.5f > 0.25f)
                this.GetComponent<SpriteRenderer>().color = Color.red;
            else
                this.GetComponent<SpriteRenderer>().color = raw_Color;
        }
        else if (enduranceBar <= 25)
        {

            blink_Time_1 += Time.deltaTime;
            if (blink_Time_1 % 0.3f > 0.15f)
                this.GetComponent<SpriteRenderer>().color = Color.red;
            else
                this.GetComponent<SpriteRenderer>().color = raw_Color;
        }
        else
        {
            blink_Time_1 = 0;
            this.GetComponent<SpriteRenderer>().color = raw_Color;
        }

    }

    void Play_SFX_Walk()
    {
        _audioSource[0].PlayOneShot(SFX_Walk,1);
    }
    void Play_SFX_Walk2()
    {
        _audioSource[0].PlayOneShot(SFX_Walk2,1);
    }

    void Play_SFX_Jump_Begin()
    {
        if (_SFX_Pernament)
        {
            //Debug.Log("Bounce Jump");
            _SFX_Pernament = false;
        }
        else if (!wallJumped)
        {
            _audioSource[0].PlayOneShot(SFX_Jump, 1);
            _audioSource[1].PlayOneShot(SFX_JumpA, 1);
        }
        else
        {
            _audioSource[0].PlayOneShot(SFX_WallJump,1);
        }
    }

    void Play_SFX_Jump_Over()
    {
        _audioSource[0].PlayOneShot(SFX_Jump2, 1);
    }

    void Play_SFX_Climb()
    {
        _audioSource[0].PlayOneShot(SFX_Climb,1);
    }

   void New_Move_Is_True()
    {
        New_Can_Move = true;
    }
    void New_Move_Is_False()
    {
        ghost.makeGhost = false;
        rb.velocity = Vector3.zero;
        anim.SetHorizontalMovement(0, 0, rb.velocity.y);
        isDashing = false;
        New_Can_Move = false;
    }


}
