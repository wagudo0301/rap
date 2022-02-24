using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTemplate : MonoBehaviour
{
    GameObject MyCanvasForRPG;
    GameObject MyTalkingNPC;
    string parameter;

    void Awake()
    {
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyTalkingNPC=MyCanvasForRPG.transform.Find("TalkingNPCMemorizer").GetComponent<TalkingNPCMemorizer>().TalkingNPC;
        //MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=false;
        parameter=MyTalkingNPC.GetComponent<NPCAnswerer>().ParameterOfFlag;

        //ここに処理を書く
    }
    void Start()
    {
        EndFunction();//これで処理を終わって会話に戻る
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
        Destroy(this);//いるはず
    }
}
