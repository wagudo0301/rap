using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchlineReflector : MonoBehaviour
{
    public float RateOfPunchline=1;
    int PunchlineNum=0;
    void Start()
    {
        if(PublicStaticStatus.Flags.Contains("Punchline1"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            PunchlineNum+=1;
        }
        if(PublicStaticStatus.Flags.Contains("Punchline2"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            PunchlineNum+=1;
        }
        RateOfPunchline*=Mathf.Pow(2,PunchlineNum);
        transform.GetChild(1).gameObject.GetComponent<Text>().text="×"+RateOfPunchline;
    }
    void Update()
    {
        
    }
}
