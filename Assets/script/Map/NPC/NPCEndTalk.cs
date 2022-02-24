using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEndTalk : MonoBehaviour
{
    GameObject MyCanvasForRPG;
    GameObject MyTalkingNPC;

    void Awake()
    {
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyTalkingNPC=MyCanvasForRPG.transform.Find("TalkingNPCMemorizer").GetComponent<TalkingNPCMemorizer>().TalkingNPC;

        MyTalkingNPC.GetComponent<NPCAnswerer>().page=MyTalkingNPC.GetComponent<NPCAnswerer>().MainText.Count-1;
    }
    void Start()
    {
        EndFunction();//これで処理を終わって会話に戻る
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
        Destroy(this);
    }
}
