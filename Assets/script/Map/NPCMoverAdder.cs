using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoverAdder : MonoBehaviour
{
    NPCMover MyNPCMover;
    void Start()
    {
        MyNPCMover=gameObject.GetComponent<NPCMover>();
    }

    void Update()
    {
        /*MoveCommandList.Add((1,0,3));
        MoveCommandList.Add((-1,0,4));
        MoveCommandList.Add((0,1,1));
        MoveCommandList.Add((0,-1,1));*/
    }
}
