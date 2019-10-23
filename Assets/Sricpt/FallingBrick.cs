using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBrick : MonoBehaviour
{
    public float falling_time ;
    public float time_to_back;
    public float static_falling_time = 2f;
    public float static_time_to_back = 3f;
    public bool is_back;
    public bool is_rock;
    private Collider2D cd;
    private Animator ani;
    private SpriteRenderer sr;
    public ParticleSystem falling_particle;
    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponentInChildren<Collider2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent < SpriteRenderer > ();
        time_to_back = static_time_to_back;
        falling_time = static_falling_time;
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("Is_Rock", is_rock);
        if (is_rock)
        {
            if(!falling_particle.isPlaying)
            falling_particle.Play();
            falling_time -= Time.deltaTime;
            if (falling_time <= 0)
            {
                is_rock = false;
                cd.enabled = false;
                sr.enabled = false;
                falling_time = static_falling_time;
                is_back = true;
                falling_particle.Stop();
            }
        }
        if(is_back)
        {
            time_to_back -= Time.deltaTime;
            if(time_to_back<=0)
            {
                cd.enabled = true;
                sr.enabled = true;
                time_to_back = static_time_to_back;
                is_back = false;
            }
        }
    }


}
