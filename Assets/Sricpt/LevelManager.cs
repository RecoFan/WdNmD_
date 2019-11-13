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
    private bool _hasDead = false;
    private AudioSource _audioSource;
        
    void StartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void EndScene()
    {
        _anim.gameObject.transform.position = deadPosition.position;
        _anim.SetTrigger("OUT");
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
        SpawnIndex = other.nowMapIndex;
        deadPosition = _player.transform;
        EndScene();
        //yield return new WaitForSeconds(0.3f);
        
        Destroy(_player);
        _player = null;
        
        yield return new WaitForSeconds(1f);
        _controlFlag = true;
        _hasDead = false; 
        StartScene();

        
    }

}
