using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Activator : MonoBehaviour
{
    void Start()
    {
        if(!PublicStaticStatus.Flags.Contains("EndDay1"))
        {
            foreach(Transform tf in gameObject.transform)
            {
                tf.gameObject.SetActive(true);
            }
        }
    }
}
