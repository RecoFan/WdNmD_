using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public int SpawnIndex;
    public NewCameraMove other;
    public GameObject[] spawnPosition;
    public GameObject _player;
    public Transform deadPosition;
    private Animator _anim;
    //public RawImage rawImage;
    private bool _controlFlag = false;
        
    void StartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void EndScene()
    {
        _anim.gameObject.transform.position = deadPosition.position;
        _anim.SetTrigger("OUT");
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
           
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        other = GameObject.FindWithTag("MainCamera").GetComponent<NewCameraMove>();
        spawnPosition = GameObject.FindGameObjectsWithTag("Respawn");
        _player = GameObject.FindWithTag("Player");
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //放在重载后太快，无法检测到生成物体
        if (_controlFlag)
        {
            other = GameObject.FindWithTag("MainCamera").GetComponent<NewCameraMove>();
            spawnPosition = GameObject.FindGameObjectsWithTag("Respawn");
            _player = GameObject.FindWithTag("Player");
            _player.transform.position = spawnPosition[SpawnIndex].transform.position;
            _controlFlag = false;
        }

        if (_player && _player.GetComponent<Movement>().isDeath)
        {
            SpawnIndex = other.nowMapIndex;
            deadPosition = _player.transform;
            _controlFlag = true;
            Destroy(_player);
            _player = null;
            EndScene();
            StartScene();
        }
    }

}
