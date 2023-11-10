using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiConrol_LSH : MonoBehaviour
{   
    public void FadeIn()
    {
        UIManager.Instance.isCloud = true;
    }

    public void FadeOut()
    {
        CameraManager.Instance.endFadeOut = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
