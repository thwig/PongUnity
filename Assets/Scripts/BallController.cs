using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;


public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    private const int DIRECTION_CHANGER = -1;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    [SerializeField] private Camera bounds;
    [SerializeField] private float iHat = 1, jHat = 1, defaultSpeedScalar, speedReducer = 0.5f;
    private float speedScalar;
    private bool hasBouncedOnce = false;
    private static Vector2 storedVelocity;
    public static Vector2 StoredVelocity
    {
        get { return storedVelocity; }
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasBouncedOnce) speedScalar = defaultSpeedScalar;
        rb.velocity = new(iHat, jHat);
        rb.velocity *= speedScalar;
        storedVelocity = rb.velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.WallTag))
        {
            Debug.Log("Hit the wall");
            jHat *= DIRECTION_CHANGER;
            if (speedScalar > defaultSpeedScalar) speedScalar *= speedReducer;
            else speedScalar = defaultSpeedScalar;
            hasBouncedOnce = true;

        }
        else if (collision.gameObject.CompareTag(Tags.P1GoalTag))
        {
            Scoring.P1Score++;
            Debug.Log("Player 1 Score: " + Scoring.P1Score);
        }
        else if (collision.gameObject.CompareTag(Tags.P2GoalTag))
        {
            Scoring.P2Score++;
            Debug.Log("Player 2 Score: " + Scoring.P2Score);
        }
        else if (collision.gameObject.CompareTag(Tags.PaddleTag))
        {
            Debug.Log("Hit a paddle");
            iHat *= DIRECTION_CHANGER;
            if (Mathf.Sign(PlayerPaddleController.StoredVelocity.y) == -1) jHat *= DIRECTION_CHANGER;
            speedScalar += Mathf.Abs(PlayerPaddleController.StoredVelocity.y) * speedReducer;
        }
    }

    
}
