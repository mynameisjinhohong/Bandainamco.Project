using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene1_LSH : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextScene;
    void scene1Animation()
    {
        nextScene.SetActive(true);
        gameObject.SetActive(false); 
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
