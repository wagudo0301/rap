using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnswerer : MonoBehaviour//会話にスクリプトを入れる場合はMainTextに書く。その際、_から始めて、その次にパラメーターを入れる。
{
    public string NameText="";
    public List<string> MainText=new List<string>();
    public string ParameterOfFlag;

    public int page;//public
    public bool CanCheckNextPage=true;//
    [SerializeField]
    private Sprite Spr;
    [SerializeField]
    private AudioClip sound;
    GameObject Player;
    GameObject MyCanvasForRPG;
    GameObject MyFadeOuter;
    GameObject go0;
    GameObject go1;
    Image MyImage;

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
                StartTalk();
                CheckNextPage();
            }
            //次ページをチェックして表示
            else if(page>=0)
            {
                CheckNextPage();
            }
        }
    }
    public void CheckNextPage()//public
    {
        if(!CanCheckNextPage){return;}
        gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
        if(MainText.Count<=page+1)//ページがめくれないなら表示終了
        {
            page=-1;
            EndTalk();
        }
        else if(MainText[page+1].Substring(0, 1)=="_")//メッセージの代わりに_で始まったらスクリプト
        {
            page+=2;
            ParameterOfFlag=MainText[page];
            CanCheckNextPage=false;
            gameObject.AddComponent(Type.GetType(MainText[page-1].Substring(1)));
            //スクリプトはCanCheckNextPageをtrueにしてページをめくる（必ずNPCTemplate参考にする）
        }
        else//次ページにする
        {
            page+=1;
            go1.GetComponent<Text>().text=MainText[page];
        }
    }
    public void StartTalk()
    {
        Debug.Log("Talk");
        Time.timeScale=0f;
        Player.GetComponent<Player>().ControlEnable=false;
        MyCanvasForRPG.transform.Find("TalkingNPCMemorizer").GetComponent<TalkingNPCMemorizer>().TalkingNPC=gameObject;
        MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 170);
        MyImage.sprite=Spr;
        MyImage.color=new Color32 (255,255,255,255);                
        if(NameText!="")
        {
            MyCanvasForRPG.transform.Find("NameTextPanel").gameObject.SetActive(true);
            go0=MyCanvasForRPG.transform.Find("NameTextPanel").transform.Find("NameText").gameObject;
            go0.GetComponent<Text>().text=NameText;
        }
        MyCanvasForRPG.transform.Find("MainTextPanel").gameObject.SetActive(true);
        MyCanvasForRPG.transform.Find("NextPageIcon").gameObject.SetActive(true);
        go1=MyCanvasForRPG.transform.Find("MainTextPanel").transform.Find("MainText").gameObject;
    }
    public void EndTalk()
    {
        Debug.Log("EndTalk");
        MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, 0);
        MyImage.color=new Color32 (0,0,0,0);
        if(go0){go0.GetComponent<Text>().text="";}
        go1.GetComponent<Text>().text="";
        MyCanvasForRPG.transform.Find("NameTextPanel").gameObject.SetActive(false);
        MyCanvasForRPG.transform.Find("MainTextPanel").gameObject.SetActive(false);
        MyCanvasForRPG.transform.Find("NextPageIcon").gameObject.SetActive(false);
        Time.timeScale=1.0f;
        Player.GetComponent<Player>().ControlEnable=true;
    }
}
