using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnswerer : MonoBehaviour
{
    GameObject Player;
    GameObject FadeOuter;
    void Start()
    {
        Player=GameObject.Find("Player");
        FadeOuter=GameObject.Find("FadeOuter");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(gameObject.transform.position,Player.transform.position)<=1.4f)
            {
                Debug.Log("talk");
                Time.timeScale=0f;
                FadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 170);
                //まだポーズ中に頭ぐりぐりできる
            }
        }
    }
}
