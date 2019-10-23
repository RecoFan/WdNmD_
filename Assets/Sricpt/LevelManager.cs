using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    private Animator _anim;
    public Transform spawnPosition;
    private GameObject _player;
    public RawImage rawImage;
    //private bool _controlFlag = false;
    
    private void StartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
    }
    private void EndScene()
    {
        _anim.SetTrigger("OUT");
    }
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _player.transform.position = spawnPosition.position;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_player && _player.GetComponent<Movement>().isDeath)
        {
            Destroy(_player);
            _player = null;
            EndScene();
        }
    }
    
}
