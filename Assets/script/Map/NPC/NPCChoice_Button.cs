using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChoice_Button : MonoBehaviour
{
    public NPCChoice MyNPCChoice;
    public void OnClick()
    {
        MyNPCChoice.ButtonChosen(gameObject.name);
    }
}
