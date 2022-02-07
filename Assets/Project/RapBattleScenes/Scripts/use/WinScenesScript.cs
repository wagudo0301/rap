using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WinScenesScript : MonoBehaviour
{
    [SerializeField]
    private Text kumaiResultPonit;
    [SerializeField]
    private Text mashiroResultPonit;
    // Start is called before the first frame update
    void Start()
    {
        kumaiResultPonit.text = PublicStaticStatus.KumaiResultPoint.ToString();
        mashiroResultPonit.text = PublicStaticStatus.MashiroResultPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(PublicStaticStatus.OnlyRapBattle) SceneManager.LoadScene("TitleScenes");
            else SceneManager.LoadScene("Chapter1-after_the_battleScenes");
        }   
    }
}
