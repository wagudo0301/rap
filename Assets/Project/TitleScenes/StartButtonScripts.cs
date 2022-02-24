using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickStartButton()
    {
        PlayerPrefs.SetInt("SAVE", 0);
        PlayerPrefs.Save();
        PublicStaticStatus.OnlyRapBattle = false;
        PublicStaticStatus.Flags.Clear();
        SceneManager.LoadScene("PrologueScenes");
    }
}
