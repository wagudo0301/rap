using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnswerer_buckup : MonoBehaviour
{
    public string NameText="";
    public List<string> MainText=new List<string>();
    public int page;

    [SerializeField]
    private Sprite Spr;
    GameObject Player;
    GameObject MyCanvasForRPG;
    GameObject MyFadeOuter;
    GameObject go0;
    GameObject go1;
    Image MyImage;
    bool TurnPageIsActive=true;

    void Start()
    {
        Player=GameObject.Find("Player");
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyFadeOuter=GameObject.Find("FadeOuter");
        MyImage=MyCanvasForRPG.transform.Find("Image").GetComponent<Image>();

        page=-1;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //最初ページを表示
            if(page==-1&&Player.GetComponent<Player>().ControlEnable&&Vector3.Distance(gameObject.transform.position,Player.transform.position)<=1.67f)
            {
                Debug.Log("Talk");
                Time.timeScale=0f;
                Player.GetComponent<Player>().ControlEnable=false;
                MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 170);
                MyImage.sprite=Spr;
                MyImage.color=new Color32 (255,255,255,255);                
                if(NameText!="")
                {
                    MyCanvasForRPG.transform.Find("NameTextPanel").gameObject.SetActive(true);
                    go0=GameObject.Find("NameText").gameObject;
                    go0.GetComponent<Text>().text=NameText;
                }
                MyCanvasForRPG.transform.Find("MainTextPanel").gameObject.SetActive(true);
                MyCanvasForRPG.transform.Find("NextPageIcon").gameObject.SetActive(true);
                go1=GameObject.Find("MainText").gameObject;

                TurnPage();

            }
            //次ページをチェックして表示
            else if(page>=0)
            {
                if(MainText.Count>page+1)//2ページ目以降表示
                {
                    TurnPage();
                }
                else if(MainText.Count<=page+1)//ページがめくれないなら表示終了
                {
                    Debug.Log("EndTalk");
                    EndTalk();
                }
            }
        }
    }
    void TurnPage()
    {
        if(!TurnPageIsActive){return;}
        page+=1;

        //Flagチェック
        if(MainText[page].Substring(0, 1)=="_")//メッセージの代わりに_で始まったらスクリプト
        {
            ChangeFlag();
            
            //次ページをチェックして表示
            if(MainText.Count>page+1)
            {
                TurnPage();
            }
            else if(MainText.Count<=page+1)
            {
                Debug.Log("EndTalk");
                EndTalk();
            }
        }
        //次ページにする
        else
        {
            go1.GetComponent<Text>().text=MainText[page];
        }
    }
    public void EndTalk()
    {
        MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 0);
        MyImage.color=new Color32 (0,0,0,0);
        if(go0){go0.GetComponent<Text>().text="";}
        go1.GetComponent<Text>().text="";
        MyCanvasForRPG.transform.Find("NameTextPanel").gameObject.SetActive(false);
        MyCanvasForRPG.transform.Find("MainTextPanel").gameObject.SetActive(false);
        MyCanvasForRPG.transform.Find("NextPageIcon").gameObject.SetActive(false);
        Time.timeScale=1.0f;
        Player.GetComponent<Player>().ControlEnable=true;

        page=-1;
    }
    void ChangeFlag()
    {
        gameObject.AddComponent(Type.GetType(MainText[page].Substring(1)));
    }
}
