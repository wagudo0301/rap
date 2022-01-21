using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class PointBar : MonoBehaviour
{
    Slider hpSlider;
 
    // Use this for initialization
    float timer;
    void Start()
    {
 
        hpSlider = GetComponent<Slider>();
 
        float maxHp = 100f;
        float nowHp = 100f;
 
 
        //スライダーの最大値の設定
        hpSlider.maxValue = maxHp;
 
        //スライダーの現在値の設定
        hpSlider.value = nowHp;
 
        
    }
 

    // Update is called once per frame
    void Update()
    {   
        timer+=Time.deltaTime;
        hpSlider.value -= timer;
        // if(timer>=2){
        //     hpSlider.value = 50f;
        //     Debug.Log("変更1");
        // }
        // if(timer>=3){
        //     hpSlider.value = 40f;
        //      Debug.Log("変更2");
        // }
        // if(timer>=4){
        //     hpSlider.value = 30f;
        //     Debug.Log("変更3");
        // }
        // if(timer>=5){
        //     hpSlider.value = 20f;
        //     Debug.Log("変更4");
        // }
        // if(timer>=6){
        //     hpSlider.value = 0f;
        //     Debug.Log("変更5");
        // }
        
    }
    
}
