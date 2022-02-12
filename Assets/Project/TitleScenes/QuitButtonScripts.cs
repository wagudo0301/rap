using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuitButtonScripts : MonoBehaviour
{
    public void ButtonExit()
    {
        //Application.Quit();
        SceneManager.LoadScene("TitleScenes");
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
