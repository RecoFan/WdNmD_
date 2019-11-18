using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DegreeController : MonoBehaviour
{
    public NewCameraMoveLevel2 CameraMoveLevel2;
    public Transform[] DegreeMask;
    public int _index_on;

    // Start is called before the first frame update
    void Start()
    {
        CameraMoveLevel2 = GameObject.FindObjectOfType<NewCameraMoveLevel2>();
        DegreeMask = GetComponentsInChildren<Transform>();
        _index_on = CameraMoveLevel2.cameraNowMapIndex;
        foreach (var VARIABLE in DegreeMask)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        DegreeMask[0].gameObject.SetActive(true);
        DegreeMask[_index_on+1].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraMoveLevel2.cameraNowMapIndex != _index_on)
        {
            DegreeMask[_index_on+1].gameObject.SetActive(false);
            _index_on = CameraMoveLevel2.cameraNowMapIndex;
            DegreeMask[_index_on+1].gameObject.SetActive(true);
        }
    }
}
