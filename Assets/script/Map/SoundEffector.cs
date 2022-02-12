using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioClip sound;
    float timer;
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
    }

    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>=3.0f)
        {
            Destroy(gameObject);
        }
    }
}
