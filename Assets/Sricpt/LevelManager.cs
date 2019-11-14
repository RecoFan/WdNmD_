using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int SpawnIndex = 0;
    public NewCameraMove other;
    public GameObject[] spawnPosition;
    public GameObject _player;
    public Transform deadPosition;
    private Animator[] _anim;
    private bool _controlFlag = false;
    private bool _hasDead = false;
    private AudioSource _audioSource;
    public bool Move_State;
    void StartScene()
    {
        Move_State = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _anim[1].SetTrigger("IN");
    }
    void EndScene()
    {
        _anim[0].gameObject.transform.position = deadPosition.position;
        _anim[0].SetTrigger("OUT");
        _audioSource.Play();
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

        _audioSource = GetComponent<AudioSource>();
        other = GameObject.FindWithTag("MainCamera").GetComponent<NewCameraMove>();
        spawnPosition = GameObject.FindGameObjectsWithTag("Respawn");
        _player = GameObject.FindWithTag("Player");
        _anim = GetComponentsInChildren<Animator>();
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

        if (_player && _player.GetComponent<Movement>().isDeath && !_hasDead )
        {
            /*SpawnIndex = other.nowMapIndex;
            deadPosition = _player.transform;
            _controlFlag = true;
            EndScene();
            Destroy(_player);
            _player = null;
            StartScene();*/
            StartCoroutine(Event());
        }
    }
    IEnumerator Event()
    {
        _hasDead = true;
        //SpawnIndex = other.nowMapIndex;
        deadPosition = _player.transform;
        EndScene();
        Destroy(_player);
        _player = null;
        yield return new WaitForSeconds(0.5f);
        _anim[1].SetTrigger("OUT");
        
        yield return new WaitForSeconds(0.75f);
        _controlFlag = true;
        _hasDead = false; 
        StartScene();

        
    }

}
