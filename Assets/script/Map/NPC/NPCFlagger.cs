using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCFlagger : MonoBehaviour
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
        PublicStaticStatus.Flags.Add(parameter);//パラメーターはフラグの名前
        Debug.Log("フラグ");
        foreach(string st in PublicStaticStatus.Flags)
        {
            Debug.Log(st);
        }
    }

    void Start()
    {
        EndFunction();
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
        Destroy(this);//いるはず
    }
}
