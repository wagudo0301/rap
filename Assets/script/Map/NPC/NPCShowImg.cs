using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCShowImg : MonoBehaviour//パラメーターは「リソースからロードする画像名」にしたい
{
    GameObject MyCanvasForRPG;
    GameObject MyTalkingNPC;
    string parameter;
    GameObject go;

    void Awake()
    {
        MyCanvasForRPG=GameObject.Find("CanvasForRPG");
        MyTalkingNPC=MyCanvasForRPG.transform.Find("TalkingNPCMemorizer").GetComponent<TalkingNPCMemorizer>().TalkingNPC;
        //MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=false;
        parameter=MyTalkingNPC.GetComponent<NPCAnswerer>().ParameterOfFlag;//パラメーターは「ロードするリソース名」

        //ここに処理を書く
        go=Instantiate(Resources.Load<GameObject>(parameter), new Vector3( 0,0,0), Quaternion.identity);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(go);
            EndFunction();//これで処理を終わって会話に戻る
        }
    }
    void EndFunction()
    {
        MyTalkingNPC.GetComponent<NPCAnswerer>().CanCheckNextPage=true;
        MyTalkingNPC.GetComponent<NPCAnswerer>().CheckNextPage();
        Destroy(this);
    }
}
