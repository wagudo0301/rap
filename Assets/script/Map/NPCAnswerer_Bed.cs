using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnswerer_Bed : MonoBehaviour
{
    public string NameText="";
    //public string MainText="";
    public List<string> MainText=new List<string>();
    [SerializeField]
    private Animator Anim;
    [SerializeField]
    private Sprite Spr;

    GameObject Player;
    GameObject MyCanvasForRPG;
    GameObject MyFadeOuter;
    GameObject go0;
    GameObject go1;
    Image MyImage;
    int page;
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

            //1ページ目表示
            if(Vector3.Distance(gameObject.transform.position,Player.transform.position)<=1.67f&&page==-1&&Player.GetComponent<Player>().ControlEnable)
            {
                Debug.Log("Talk");
                page+=1;

                Time.timeScale=0f;//まだポーズ中に頭ぐりぐりできる
                Player.GetComponent<Player>().ControlEnable=false;
                Debug.Log("nocon");
                //Anim.SetFloat("X",0);
                //Anim.SetFloat("Y",-1);
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
                go1.GetComponent<Text>().text=MainText[page];

            }
            //2ページ目以降表示
            else if(page>=0)
            {
                page+=1;
                //2ページ目以降表示
                if(MainText.Count>page)
                {
                    go1.GetComponent<Text>().text=MainText[page];
                }
                //ページがめくれないなら表示終了
                if(MainText.Count<=page)
                {
                    Debug.Log("EndTalk");
                    EndTalk();
                }
            }
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
        Debug.Log("yescon");

        page=-1;

        
        if(gameObject.GetComponent<NPCFlagger_Bed>())
        {
            Debug.Log("Flag");
            {
                gameObject.GetComponent<NPCFlagger_Bed>().Flag=true;
            }
        }
    }
}
