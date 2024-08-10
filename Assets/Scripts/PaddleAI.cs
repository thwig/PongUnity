using UnityEngine;
using PongUnity.CoreConstants;

public class PaddleAI : MonoBehaviour
{
    [SerializeField] private float ReactionTime = 0.2f;
    // Start is called before the first frame update
    [SerializeField] float difficulty;
    [SerializeField] float range;
    float predictorTweak = 0.5f;

    // Globals
    private float _newPos;
    private float target;
    // Components
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    
    void Start()
    {
        Paddles.InitializePaddlePosition(gameObject.transform);
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BallController.CrossedNet && BallController.StoredVelocity.x > 0)
        {
            OnNetCross();
        }
        else if (BallController.CrossedNet && BallController.StoredVelocity.x < 0) 
        {
            target = NumericConstants.OriginY;
        }
        float smoother = Mathf.Lerp(_rb.position.y, target, predictorTweak);
        _rb.position = new Vector2(_rb.position.x, smoother);         
    }

    void OnNetCross()
    {
        target = PredictedPosition();
    }

    
    float PredictedPosition()
    {
        Vector2 predictedPosition = BallController.StoredPosition;
        Vector2 predictedVelocity = BallController.StoredVelocity;

        float elapsedTime = 0f;

        float ceilingBound = Bounds.TopPos.y - Bounds.BoundsWidth - _bc.size.y;
        float floorBound = Bounds.BottomPos.y + Bounds.BoundsWidth + _bc.size.y;

        while (elapsedTime < ReactionTime)
        {
            predictedPosition += predictedVelocity * Time.fixedDeltaTime;
            elapsedTime += Time.fixedDeltaTime;
            
            if (predictedPosition.y >= ceilingBound || predictedPosition.y <= floorBound)
            {
                predictedVelocity.y *= -1;
                predictedPosition.y = Mathf.Clamp(predictedPosition.y, floorBound, ceilingBound);
            }
            if (predictedPosition.x >= gameObject.transform.position.x)
            {
                break;
            }
            
        }

        return predictedPosition.y;
    }

    
    
}
