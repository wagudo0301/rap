using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCFlagger : MonoBehaviour
{
    NPCAnswerer MyNPCAnswerer;
    void Start()
    {
        MyNPCAnswerer=gameObject.GetComponent<NPCAnswerer>();
        PublicStaticStatus.Flags.Add(MyNPCAnswerer.ParameterOfFlag);
        Debug.Log("フラグ");
        foreach(string st in PublicStaticStatus.Flags)
        {
            Debug.Log(st);
        }
        Destroy(this);
    }
}
