using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using PongUnity.CoreConstants;
using System.Collections;

public class PaddleAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float difficulty;
    [SerializeField] float range;
    float predictorTweak = 0.1f;
    private const int MaxBounces = 10;
    // Globals
    private Vector2 ballPosition;
    private Vector2 ballVelocity;
    private float _newPos;
    private float _target;
    private int bounces;
    private bool netFlag = false;
    private Vector2 bounds;
    

    // Components
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;

    private void Awake()
    {
        OnNetCross();
    }
    void Start()
    {
        Paddles.InitializePaddlePosition(gameObject.transform);
        UpdateBallVelocity();
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        bounds = new Vector2(Bounds.BottomPos.y + _bc.size.y, Bounds.TopPos.y - _bc.size.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateBallVelocity();

        if (HasCrossedNet() == 1 && !netFlag)
        {
            netFlag = true;
            OnNetCross();
        }
        else if (HasCrossedNet() == 0) 
        {
            netFlag = false;
            _target = NumericConstants.OriginY;
        }
        float smoother = Mathf.Clamp(Mathf.Lerp(_rb.position.y, _target, 0.5f), bounds.x, bounds.y);
        Vector2 newPos = new Vector2(_rb.position.x, smoother);
        _rb.position = newPos;         
    }

    int HasCrossedNet()
    {
        if (BallController.CrossedNet && ballVelocity.x > 0)
        {
            return 1;
        }
        else if (BallController.CrossedNet && ballVelocity.x < 0)
        {
            return 0;
        }
        return -1;
    }
    void OnNetCross()
    {
        Debug.Log("Crossed Net");
        _target = GhostBallPredict();

    }

    float GhostBallPredict()
    {
        Vector2 origin = BallController.StoredPosition;
        Vector2 direction = BallController.StoredVelocity.normalized;

        WaitTime(predictorTweak);

        return RayBounce(origin, direction).y;
    }
    Vector2 RayBounce(Vector2 origin, Vector2 direction)
    {
        float maxDistance = Mathf.Infinity;
        Vector2 rayInit = new Vector2(origin.x, origin.y);
        RaycastHit2D bouncingRay = Physics2D.Raycast(rayInit, direction, maxDistance);
        Debug.DrawRay(origin, direction, Color.red, 2f);

        if (bounces < MaxBounces)
        {
            switch (bouncingRay.collider.tag)
            {
                case Tags.P2GoalTag:
                    return bouncingRay.point;

                case Tags.WallTag:
                    Debug.Log("Hit!" + bouncingRay.point);
                    bounces++;

                    Vector2 reflectedDirection = Vector2.Reflect(direction, bouncingRay.normal);
                    Vector2 newOrigin = bouncingRay.point + reflectedDirection * predictorTweak;

                    return RayBounce(newOrigin, reflectedDirection);

                default:
                    return Vector2.zero;
            }
        }
        else return Vector2.zero;
    }

    void WaitTime(float seconds)
    {
        float MaxTime = seconds;
        float elapsedTime = 0f;

        while (elapsedTime < MaxTime)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void UpdateBallVelocity()
    {
        ballPosition = BallController.StoredPosition;
        ballVelocity = BallController.StoredVelocity;
    }
}
