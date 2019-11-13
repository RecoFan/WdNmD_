using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class ChangeMask : MonoBehaviour
{
    // Start is called before the first frame update
    PostProcessVolume m_Volume;
    ChromaticAberration ca;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MaskTrue()
    {
        this.GetComponent<SpriteMask>().enabled = true;
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(0.2f, 2f, 14, 90, false, true);
        ca = ScriptableObject.CreateInstance<ChromaticAberration>();
        ca.enabled.Override(true);

        DOVirtual.Float(0.15f, 0.50f, 0.2f, ca.intensity.Override);

        m_Volume = PostProcessManager.instance.QuickVolume(12, 0f, ca);
    }
    void MaskFalse()
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(0.1f, 2f, 14, 90, false, true);
        DOVirtual.Float(0.50f, 0.15f, 0.1f, ca.intensity.Override).OnComplete(DestroyVolume);
        this.GetComponent<SpriteMask>().enabled = false;
    }
    void DestroyVolume()
    {

        RuntimeUtilities.DestroyVolume(m_Volume, true, true);

    }
}
