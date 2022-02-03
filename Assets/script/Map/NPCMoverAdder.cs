using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoverAdder : MonoBehaviour
{
    NPCMover MyNPCMover;
    List<(int,int,float)> PossibleMoveCommand=new List<(int,int,float)>(){ (-1,0,1), (1,0,1), (0,-1,1), (0,1,1)};
    float timer;
    void Start()
    {
        MyNPCMover=gameObject.GetComponent<NPCMover>();
        StartCoroutine(loop());
    }
    private IEnumerator loop()
    {
        while (true)
        {
            MyNPCMover.MoveCommandList.Add(PossibleMoveCommand[Random.Range(0,  PossibleMoveCommand.Count)]);
            yield return new WaitForSeconds(1.0f);
            MyNPCMover.MoveCommandList.Add((0,0,1));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
