using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOnSoundPlay_HJH : MonoBehaviour
{
    AudioSource myaudio;
    bool play = false;
    // Start is called before the first frame update
    void Start()
    {
        myaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!play)
        {
            if(Time.timeScale > 0)
            {
                play = true;
                myaudio.Play();
            }

        }
    }
}
