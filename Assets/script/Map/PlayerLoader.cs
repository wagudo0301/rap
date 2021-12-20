using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public GameObject Player;
    void Awake()
    {
        if(!GameObject.Find("Player"))
        {
            GameObject go=Instantiate(Player);
            go.name="Player";
        }
        if(PublicStaticStatus.LastTeleportDistination=="")
        {
            Debug.Log("distination empty");
        }
        else
        {
            GameObject.Find("Player").transform.position=GameObject.Find(PublicStaticStatus.LastTeleportDistination).transform.position;
        }
    }
}
