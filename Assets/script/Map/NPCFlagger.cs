using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFlagger : MonoBehaviour
{
    public bool Flag=false;
    bool FlagChanged=false;
    public string FlagName;

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
        }
    }
}
