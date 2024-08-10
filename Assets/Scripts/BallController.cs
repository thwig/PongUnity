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
    
    // Static Fields
    private static Vector2 _storedVelocity;
    private static Vector2 _storedPosition;
    private static int _bounceCounter;
    private static bool _crossedNet;
    

    
    // Fields
    private float _speedScalar;
    private int _velDirection;
    private float currentHorizontalDirection, currentVerticalDirection;
    // Components
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;
    private SpriteRenderer _sr;

    // Properties
    public static int BounceCounter { get => _bounceCounter; }
    public static Vector2 StoredVelocity { get => _storedVelocity; }
    public static Vector2 StoredPosition { get => _storedPosition; }
    public static bool CrossedNet { get => _crossedNet; set => _crossedNet = value; }
    
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _circleCollider = gameObject.GetComponent<CircleCollider2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _circleCollider.radius = _sr.size.x - 0.1f;
        _speedScalar = defaultSpeedScalar;
        CrossedNet = true;
        DefaultBallVelocity();
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
                HandleGoalCollision(Tags.P2GoalTag);
                Debug.Log("Player 2 Score: " + Scoring.P2Score);
                
                UpdateBallVelocity();
                break;
            
            case Tags.NetTag:
                HandleNetCollision();
                break;
        }
    }

    void OnTriggerExit2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Tags.NetTag:
                _crossedNet = false;
                break;
        }
    }

    void HandleWallCollision()
    {
        currentVerticalDirection *= DirectionChangeMultiplier;
        if (_speedScalar > defaultSpeedScalar) _speedScalar *= speedReducer;
        else _speedScalar = defaultSpeedScalar;
        
        _bounceCounter++;

        UpdateBallVelocity();   
    }
    void HandlePaddleCollision()
    {
        currentHorizontalDirection *= DirectionChangeMultiplier;
        float paddleVelocity = PlayerPaddleController.StoredPaddleVelocity;
        if (paddleVelocity == 0)
        {
            currentVerticalDirection *= Mathf.Sign(paddleVelocity);
            UpdateBallVelocity();
            return;
        }
        _velDirection = (int) Mathf.Sign(paddleVelocity);
        _speedScalar += Mathf.Abs(paddleVelocity) * paddleVelocityTweak;
        if (_velDirection >= 0)
        { 
            currentVerticalDirection = -Mathf.Abs(currentVerticalDirection) * DirectionChangeMultiplier;
        }
        else 
        {
            currentVerticalDirection = Mathf.Abs(currentVerticalDirection) * DirectionChangeMultiplier;
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
                break;

            case Tags.P2GoalTag:
                Scoring.P2Score++;
                RestoreBallPosition();
                break;
        }
    }

    void HandleNetCollision()
    {
        Debug.Log("Hit the net");
        _crossedNet = true;
    }
    void RestoreBallPosition()
    {
        gameObject.transform.position = new Vector2(0f, 0f);
        
    }
    void DefaultBallVelocity()
    {
        _rb.velocity = new Vector2(horizontalDirection, verticalDirection).normalized * _speedScalar;
        currentHorizontalDirection = horizontalDirection;
        currentVerticalDirection = verticalDirection;
    }
    void UpdateBallVelocity()
    {
        if (_speedScalar < defaultSpeedScalar)
        {
            _speedScalar = defaultSpeedScalar;
        }
        _rb.velocity = new Vector2(currentHorizontalDirection, currentVerticalDirection).normalized * _speedScalar;
        _storedVelocity = _rb.velocity;
        _storedPosition = _rb.position;
    }

    
}
