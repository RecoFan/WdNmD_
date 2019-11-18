using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider Slider_progress;
    public GameObject loadingscene;
    public LevelManager Manager;

    public void LoadScene()
    {
        Debug.Log("Load entered!");
        StartCoroutine(AsyncLoading(1));
    }

    IEnumerator AsyncLoading(int sceneindex)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneindex);
        loadingscene.SetActive(true);
        Destroy(Manager.gameObject);
        while (!loading.isDone)
        {
            float progress = Mathf.Clamp01(loading.progress / 0.9f);
            Slider_progress.value = progress;
            Debug.Log(loading.progress);
            yield return null;
        }
        
        
        loadingscene.SetActive(false);
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
