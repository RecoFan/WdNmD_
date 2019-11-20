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
    public AudioSource audiosource;

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
    
    public void audiofade()
    {
        StartCoroutine(StartFade(audiosource, 2f, 0f));
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindObjectOfType<LevelManager>();
        audiosource = Manager.gameObject.GetComponentsInChildren<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
