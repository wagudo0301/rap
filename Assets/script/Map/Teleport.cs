using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string SceneName;
    public string TeleportDistinationName;
    public GameObject SoundEffector;
    private float timer=0f;

    void Update()
    {
        timer+=Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name!="Player") return;
        if(timer<=0.64f) return;
        
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"に行く");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        Instantiate(SoundEffector);
        SceneManager.LoadScene(SceneName);
        //StartCoroutine("Teleportation");
        /*
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"に行く");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        SceneManager.LoadScene(SceneName);*/
    }
    /*IEnumerator Teleportation()
    {
        yield return new WaitForSeconds(0.16f);
        PublicStaticStatus.CurrrentScene=SceneName;
        Debug.Log(PublicStaticStatus.CurrrentScene+"に行く");
        PublicStaticStatus.LastTeleportDistination=TeleportDistinationName;
        SceneManager.LoadScene(SceneName);
    }*/
}
