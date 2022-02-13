using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goTitleButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void ButtonGoTitle()
    {
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene("TitleScenes");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
