using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter1ButtonScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickChapter1Button()
    {
        int savecount = PlayerPrefs.GetInt ("SAVE",0);

        PublicStaticStatus.OnlyRapBattle = false;
        switch(savecount)
        {
            case 0:
                SceneManager.LoadScene("PrologueScenes");
                break;
            case 1:
                SceneManager.LoadScene("Chapter1-1-Scenes");
                break;
            case 2:
                SceneManager.LoadScene("Classroom1-1");
                break;
            case 3:
                SceneManager.LoadScene("Classroom1-1");
                break;
            case 4:
                SceneManager.LoadScene("Chapter1-before_the_TutorialScenes");
                break;
            case 5:
                SceneManager.LoadScene("Chapter1-before_the_battleScenes");
                break;
            case 6:
                SceneManager.LoadScene("Chapter1-after_the_battleScenes");
                break;
            case 7:
                SceneManager.LoadScene("Chapter1-EndingScenes");
                break;
            
        }
        //SceneManager.LoadScene("Classroom1-1");
        //SceneManager.LoadScene("Chapter1-1-Scenes");
    }
}
