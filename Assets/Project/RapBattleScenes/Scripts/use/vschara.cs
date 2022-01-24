using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vschara : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        gameObject.transform.localScale =new Vector3(timer,timer,1);
        gameObject.transform.Rotate(new Vector3(0f,0.06f,0f));
        if(timer>=2){
            gameObject.transform.Rotate(new Vector3(0f,1f,0f));
            if(timer>=3)
            {   
                Destroy(gameObject);
            }
    
        }
        
    }
}
