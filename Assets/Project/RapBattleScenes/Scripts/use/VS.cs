using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VS : MonoBehaviour
{
    float timer;
    void Start()
    {
        
    }

    void Update()
    {
        timer+=Time.deltaTime;
        gameObject.transform.localScale =new Vector3(timer,timer,1);
        if(timer>=3)
        {
            Destroy(gameObject);
        }
    }
}
