using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicStaticStatus : MonoBehaviour
{
    public static string CurrrentScene="";//今は使ってない
    public static string LastTeleportDistination="";
    public static float KumaiResultPoint;
    public static float MashiroResultPoint;
    public static bool OnlyRapBattle = false;

    public static List<string> Flags=new List<string>();
}
