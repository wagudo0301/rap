using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCGoToBed : MonoBehaviour//パラメーターは無い
{
    bool FlagChanged=false;
    GameObject MyFadeOuter;
    Player MyPlayer;
    bool StartFadeOutTimer=false;
    float FadeOutTimer;

    public AudioClip sound;

    void Start()
    {
        MyFadeOuter=GameObject.Find("FadeOuter");
        MyPlayer=GameObject.Find("Player").GetComponent<Player>();
        sound= Resources.Load<AudioClip>("BedSound");
    }
    void Update()
    {
        
        if(!FlagChanged)
        {
            FlagChanged=true;
            MyPlayer.ControlEnable=false;
            Debug.Log("nocon");
            gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
            Time.timeScale=1;
            StartFadeOutTimer=true;
        }
        if(StartFadeOutTimer)
        {
            Debug.Log(FadeOutTimer);
            FadeOutTimer+=Time.deltaTime;
            MyPlayer.ControlEnable=false;//デバッグ用の行
            MyFadeOuter.GetComponent<Image>().color = new Color32 (0, 0, 0, (byte)(Mathf.RoundToInt((FadeOutTimer/7.0f)*85)+170));
            if(FadeOutTimer>=7.0f)
            {
                MyPlayer.ControlEnable=true;
                Debug.Log("yescon");
                PublicStaticStatus.LastTeleportDistination="ZeroZero";
                if(!PublicStaticStatus.Flags.Contains("EndDay1"))
                {
                    PublicStaticStatus.Flags.Add("EndDay1");
                    Debug.Log("フラグ");
                    foreach(string st in PublicStaticStatus.Flags)
                    {
                        Debug.Log(st);
                    }
                    Destroy(GameObject.Find("Player"));
                    SceneManager.LoadScene("Chapter1-2-Scenes");
                }
                else if(PublicStaticStatus.Flags.Contains("EndDay1"))
                {
                    PublicStaticStatus.Flags.Add("EndDay2");
                    Debug.Log("フラグ");
                    foreach(string st in PublicStaticStatus.Flags)
                    {
                        Debug.Log(st);
                    }
                    Destroy(GameObject.Find("Player"));
                    SceneManager.LoadScene("Chapter1-before_the_TutorialScenes");
                }

            }
        }
    }
}
