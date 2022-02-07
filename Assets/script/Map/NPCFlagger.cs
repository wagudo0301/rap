using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCFlagger : MonoBehaviour
{
    public bool Flag=false;
    bool FlagChanged=false;
    public string FlagName;
    GameObject MyFadeOuter;
    Player MyPlayer;
    bool StartFadeOutTimer=false;
    float FadeOutTimer;
    void Start()
    {
        MyFadeOuter=GameObject.Find("FadeOuter");
        MyPlayer=GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        if(Flag&&!FlagChanged)
        {
            PublicStaticStatus.Flags.Add(FlagName);
            FlagChanged=true;
            /*foreach(string st in PublicStaticStatus.Flags)
            {
                Debug.Log(st);
            }*/
            if(FlagName=="GoToBed")
            {
                MyPlayer.ControlEnable=false;
                Debug.Log("nocon");
                StartFadeOutTimer=true;
            }
        }
        if(StartFadeOutTimer)
        {
            FadeOutTimer+=Time.deltaTime;
            MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, (byte)(Mathf.RoundToInt((FadeOutTimer/7.0f)*85)+170));
            if(FadeOutTimer>=7.0f)
            {
                MyPlayer.ControlEnable=true;
                Debug.Log("yescon");
                PublicStaticStatus.LastTeleportDistination="ZeroZero";
                if(!PublicStaticStatus.Flags.Contains("EndDay1"))
                {
                    PublicStaticStatus.Flags.Add("EndDay1");
                    SceneManager.LoadScene("Chapter1-2-Scenes");
                }
                else if(PublicStaticStatus.Flags.Contains("EndDay1"))
                {
                    PublicStaticStatus.Flags.Add("EndDay2");
                    SceneManager.LoadScene("Chapter1-before_the_battleScenes");
                }

            }
        }
    }
}
