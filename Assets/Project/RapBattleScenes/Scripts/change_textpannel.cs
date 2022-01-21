using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_textpannel : MonoBehaviour
{
    public Sprite kumaitextpannel;
    public Sprite mashirotextpannel;

    float timer;
    int count = 0;

    float starttime = 3.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer - starttime > 22f){
            changetextpannel(count);
            starttime = 0;
            timer = 0;
            count++;
        }
    }

    private void changetextpannel(int count){
        if(count % 2 == 0) {
            this.gameObject.GetComponent<Image> ().sprite = kumaitextpannel;
            this.gameObject.GetComponentInChildren<Text>().color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
        } else {
            this.gameObject.GetComponent<Image> ().sprite = mashirotextpannel;
            this.gameObject.GetComponentInChildren<Text>().color = new Color(0.0f, 255.0f, 0.0f, 1.0f);
        }
    }
}
