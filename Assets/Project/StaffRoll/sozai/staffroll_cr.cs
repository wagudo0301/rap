using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class staffroll_cr : MonoBehaviour
{   
    float time = 0f;
    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;//毎フレームの時間を加算.
        
        if (Input.GetMouseButtonDown(0) && time >= 5f){
            cnt += 1;
            Debug.Log(cnt+"だよ");
        }
        
        
        if (cnt == 3)
        {
           SceneManager.LoadScene("TitleScenes");
        }
        
    }
}
