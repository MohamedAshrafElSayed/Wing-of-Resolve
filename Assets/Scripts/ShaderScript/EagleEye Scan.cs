using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleEyeScan : MonoBehaviour
{
    [SerializeField] private Material _scanMat;
    [SerializeField] private Transform _scannerObj;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SceneScanning());
        }
    }

    public IEnumerator SceneScanning()
    {
        float _timer = 0;
        float _scanRange = 0;
        float _opacity = 1;
        if(_scannerObj != null)
        {
            _scanMat.SetVector("_Position", _scannerObj.position);
        }
        else
        {
            _scanMat.SetVector("_Position", this.transform.position);
        }

        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer <= 2)
            {
                _scanRange = Mathf.Lerp(0, 100, _timer);
                _opacity = Mathf.Lerp(1, 0, _timer);
                _scanMat.SetFloat("_Range", _scanRange);
                _scanMat.SetFloat("_Opacity", _opacity);
            }
            else
            {
                yield break;
            }
            yield return null;
        }
    }

}

