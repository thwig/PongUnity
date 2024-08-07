using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    private static int p1Score = 0;
    private static int p2Score = 0;

    public static int P1Score
    {
        get { return p1Score; }
        set { p1Score = value; }
    }

    public static int P2Score
    {
        get { return p2Score; }
        set { p2Score = value; }    
    }


}
