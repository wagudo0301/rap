using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battle_bgm : MonoBehaviour
{
    private AudioSource audioSource;
    private bool start = true;
    float timer;
    // Start is called before the first frame update
    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.3f && start) {
            audioSource.Play();
            start = false;
        }
    }
}
