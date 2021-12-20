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
        Debug.Log("あ");
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"aaaa");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        Debug.Log(PublicStaticStatus.LastTeleportDistination+"bbb");
        SceneManager.LoadScene(SceneName);
    }
}
