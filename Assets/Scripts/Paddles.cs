using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddles : MonoBehaviour
{
    [SerializeField] private float offset;
    private static float s_offset;
    private Vector2 _paddlePos;
    public Vector2 PaddlePos { get => _paddlePos; }
    void Awake()
    {
        s_offset = offset;
    }
    public static void InitializePaddlePosition(Transform paddle)
    {
        if (paddle.position.x < 0)
        {
            paddle.position = new Vector2(Bounds.LeftPos.x + s_offset, paddle.position.y);
        }
        else if (paddle.position.x > 0)
        {
            paddle.position = new Vector2(Bounds.RightPos.x - s_offset, paddle.position.y);
        }

    }
}