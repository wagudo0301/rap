using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCChoice : MonoBehaviour//パラメーターは「選択肢1,選択肢2,選択肢1で行く行数,選択肢2で行く行数」
{
    GameObject MyCanvasForRPG;
    GameObject go0;
    GameObject go1;
    GameObject MyTalkingNPC;
    string parameter;

    void Awake()
    {
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyTalkingNPC=MyCanvasForRPG.transform.Find("TalkingNPCMemorizer").GetComponent<TalkingNPCMemorizer>().TalkingNPC;
        parameter=MyTalkingNPC.GetComponent<NPCAnswerer>().ParameterOfFlag;

        MyCanvasForRPG.transform.Find("TopButton").gameObject.SetActive(true);
        MyCanvasForRPG.transform.Find("BottomButton").gameObject.SetActive(true);
        go0=MyCanvasForRPG.transform.Find("TopButton").gameObject;
        go1=MyCanvasForRPG.transform.Find("BottomButton").gameObject;
        go0.transform.Find("TopText").GetComponent<Text>().text=parameter.Split(',')[0];
        go1.transform.Find("BottomText").GetComponent<Text>().text=parameter.Split(',')[1];
        go0.GetComponent<NPCChoice_Button>().MyNPCChoice=this;
        go1.GetComponent<NPCChoice_Button>().MyNPCChoice=this;
    }
    public void ButtonChosen(string st)
    {
        Debug.Log(st+"で処理");
        if(st=="TopButton")
        {
            MyTalkingNPC.GetComponent<NPCAnswerer>().page=int.Parse(parameter.Split(',')[2])-1;
        }
        if(st=="BottomButton")
        {
            MyTalkingNPC.GetComponent<NPCAnswerer>().page=int.Parse(parameter.Split(',')[3])-1;
        }
        MyCanvasForRPG.transform.Find("TopButton").gameObject.SetActive(false);
        MyCanvasForRPG.transform.Find("BottomButton").gameObject.SetActive(false);
        EndFunction();
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
        Destroy(this);
    }
}
