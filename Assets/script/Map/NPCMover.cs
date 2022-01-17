using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    public List<(int,int,float)> MoveCommandList=new List<(int,int,float)>();

    [SerializeField]
    private int moveSpeed;

    [SerializeField]
    private Animator playerAnim;

    public Rigidbody2D Rb;
    
    private (int,int,float) MoveCommand=(0,0,0f);
    private float Timer=0f;

    void Start()
    {
        playerAnim.speed=0;

        MoveCommandList.Add((1,0,3));
        MoveCommandList.Add((-1,0,4));
        MoveCommandList.Add((0,1,1));
        MoveCommandList.Add((0,-1,1));
    }

    void Update()
    {
        if(MoveCommandList.Count!=0&&MoveCommand==(0,0,0f))
        {
            Debug.Log(""+MoveCommandList[0].Item1+MoveCommandList[0].Item2+MoveCommandList[0].Item3);
            MoveCommand=MoveCommandList[0];
            MoveCommandList.RemoveAt(0);

            playerAnim.SetFloat("X", MoveCommand.Item1);
            playerAnim.SetFloat("Y", MoveCommand.Item2);
            if(MoveCommand.Item1==0&&MoveCommand.Item2==0)
            {
                playerAnim.speed=0;
            }
            else
            {
                playerAnim.speed=1;
                Rb.velocity = new Vector2(MoveCommand.Item1,MoveCommand.Item2).normalized * moveSpeed;
            }
        }
        //MoveCommandあれば
        if(MoveCommand!=(0,0,0f))
        {
            Timer+=Time.deltaTime;
            if(Timer>=MoveCommand.Item3)
            {
                playerAnim.speed=0;
                Rb.velocity = new Vector2(0,0);
                MoveCommand=(0,0,0);
                Timer=0f;
            }
        }
    }
}
