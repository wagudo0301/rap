using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnswerer : MonoBehaviour
{
    public string NameText="";
    public string MainText="";
    [SerializeField]
    private Animator Anim;
    GameObject Player;
    GameObject MyCanvasForRPG;
    GameObject MyFadeOuter;
    GameObject go0;
    GameObject go1;
    void Start()
    {
        Player=GameObject.Find("Player");
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyFadeOuter=GameObject.Find("FadeOuter");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(gameObject.transform.position,Player.transform.position)<=2f)
            {
                Debug.Log("talk");
                Time.timeScale=0f;//まだポーズ中に頭ぐりぐりできる
                Anim.SetFloat("X",0);
                Anim.SetFloat("Y",-1);

                if(NameText!="")
                {
                    MyCanvasForRPG.transform.Find("NameTextPanel").gameObject.SetActive(true);
                    go0=GameObject.Find("NameText").gameObject;
                    go0.GetComponent<Text>().text=NameText;

                }
                MyCanvasForRPG.transform.Find("MainTextPanel").gameObject.SetActive(true);
                go1=GameObject.Find("MainText").gameObject;
                go1.GetComponent<Text>().text=MainText;

                MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 170);
            }
        }
    }
}
