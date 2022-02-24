using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMysterySwitch : MonoBehaviour
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
        if(GameObject.Find("MysteryChecker").GetComponent<NPCMysteryChecker>().MysetrySwitchNum==int.Parse(parameter))
        {
            Debug.Log("same");
            if(int.Parse(parameter)==4)
            {
                Debug.Log("5same");
                MyTalkingNPC.GetComponent<NPCAnswerer>().page+=2;
            }
            GameObject.Find("MysteryChecker").GetComponent<NPCMysteryChecker>().MysetrySwitchNum+=1;
        }
        else
        {
            GameObject.Find("MysteryChecker").GetComponent<NPCMysteryChecker>().MysetrySwitchNum=0;
        }
    }
    void Start()
    {
        EndFunction();//これで処理を終わって会話に戻る
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
    }
}
