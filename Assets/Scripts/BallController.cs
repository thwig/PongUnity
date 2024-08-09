using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;
using PongUnity.CoreConstants;

public class BallController : MonoBehaviour
{
    // Constants
    private const int DirectionChangeMultiplier = -1;
    
    // Fields to tweak in Unity
    [SerializeField] private Camera bounds;
    [SerializeField] private float horizontalDirection = 1, verticalDirection = 1, defaultSpeedScalar, speedReducer = 0.5f;
    [SerializeField] private float paddleVelocityTweak;
    
    // Float fields
    private float _speedScalar;
    private int _velDirection;

    // Components
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;
    private SpriteRenderer _sr;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _circleCollider = gameObject.GetComponent<CircleCollider2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _circleCollider.radius = _sr.size.x - 0.1f;
        _speedScalar = defaultSpeedScalar;
        UpdateBallVelocity();
    }
    
    // Handle collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Tags.WallTag:   
                HandleWallCollision();
                break;

            case Tags.PaddleTag:
                HandlePaddleCollision();
                break;

            case Tags.P1GoalTag:
                HandleGoalCollision(Tags.P1GoalTag);
                break;

            case Tags.P2GoalTag:
                Scoring.P2Score++;
                RestoreBallPosition();
                Debug.Log("Player 2 Score: " + Scoring.P2Score);
                
                UpdateBallVelocity();
                break;
        }
    }
    void HandleWallCollision()
    {
        verticalDirection *= DirectionChangeMultiplier;
        if (_speedScalar > defaultSpeedScalar) _speedScalar *= speedReducer;
        else _speedScalar = defaultSpeedScalar;
        
        UpdateBallVelocity();   
    }
    void HandlePaddleCollision()
    {
        horizontalDirection *= DirectionChangeMultiplier;
        _velDirection = (int) Mathf.Sign(PlayerPaddleController.StoredPaddleVelocity);
        _speedScalar += Mathf.Abs(PlayerPaddleController.StoredPaddleVelocity) * paddleVelocityTweak;
        if (_velDirection >= 0)
        { 
            verticalDirection = -Mathf.Abs(verticalDirection) * DirectionChangeMultiplier;
        }
        else 
        {
            verticalDirection = Mathf.Abs(verticalDirection) * DirectionChangeMultiplier;
        }

        UpdateBallVelocity();
    }
    void HandleGoalCollision(string goalType)
    {
        switch (goalType)
        {
            case Tags.P1GoalTag:   
                Scoring.P1Score++;
                
                RestoreBallPosition();
                UpdateBallVelocity();
                break;

            case Tags.P2GoalTag:
                Scoring.P2Score++;

                RestoreBallPosition();
                UpdateBallVelocity();
                break;
        }
    }
    void RestoreBallPosition()
    {
        gameObject.transform.position = new Vector2(0f, 0f);
        _speedScalar = defaultSpeedScalar;
        horizontalDirection *= DirectionChangeMultiplier;
        verticalDirection *= DirectionChangeMultiplier;
    }

    void UpdateBallVelocity()
    {
        _rb.velocity = new Vector2(horizontalDirection, verticalDirection).normalized * _speedScalar;

    }

    
}
