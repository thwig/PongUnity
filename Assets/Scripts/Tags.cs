using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    private static readonly string wallTag = "Wall";
    private static readonly string p1GoalTag = "Player1Goal";
    private static readonly string p2GoalTag = "Player2Goal";
    private static readonly string paddleTag = "Paddle";
    
    public static string WallTag
    {
        get { return wallTag; }
    }

    public static string P1GoalTag
    {
        get { return p1GoalTag; }
    }

    public static string P2GoalTag
    {
        get { return p2GoalTag; }
    }

    public static string PaddleTag
    {
        get { return paddleTag; }
    }

}
