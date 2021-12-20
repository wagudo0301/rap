using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string SceneName;
    public string TeleportDistinationName;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("ChangeColor");
        /*
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"に行く");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        SceneManager.LoadScene(SceneName);*/
    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.32f);
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"に行く");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        SceneManager.LoadScene(SceneName);
    }
}
