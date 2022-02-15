using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTitleScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("GoTitleCanvas"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
