using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public float PlayerPointOfRap=0;
    public float OpponentPointOfRap=0;
    public float RateOfTurn=1.0f;
    public string Rap;
    public bool WillChangeScore=false;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Text kumai_point;
    [SerializeField]
    private Text mashiro_point;

    private string stringPlayerPointOfRap;
    private battle_gamemanager Mybattle_gamemanager;

    void Start()
    {
        Mybattle_gamemanager=GameObject.Find("BattleGameManager").GetComponent<battle_gamemanager>();
    }
    void Update()
    {
        if(WillChangeScore)
        {
            float num=GameObject.Find("RapJudger").GetComponent<RapJudger>().JudgeRap(Rap);
            Debug.Log(num+"*"+RateOfTurn+"加算");
            PlayerPointOfRap+=num*RateOfTurn;
            Mybattle_gamemanager.PlayerPointOfRap+=num*RateOfTurn;
            stringPlayerPointOfRap = PlayerPointOfRap.ToString();
            kumai_point.text = stringPlayerPointOfRap;
            hpSlider.value = 1-(PlayerPointOfRap/(PlayerPointOfRap+OpponentPointOfRap));
            WillChangeScore=false;
        }
    }
}
