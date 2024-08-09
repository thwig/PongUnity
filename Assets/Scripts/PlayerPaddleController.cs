using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    // Fields for tweaking in Unity
    [SerializeField] float paddleSpeed;
    [SerializeField] float range;
    [SerializeField] float velocityTweak;

    // Float fields
    private static float storedPaddleVelocity;
    private float deltaPos;
    private float previousPos;
    private float timeInterval = 0.01f;
    private float paddleVelocity;
    private float yRange;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    // Properties
    public static float StoredPaddleVelocity
    {
        get { return storedPaddleVelocity; }
    }

    // Start is called when game initializes
    void Start()
    {
        Paddles.InitializePaddlePosition(gameObject.transform);
        rb = GetComponent<Rigidbody2D> ();
        bc = GetComponent<BoxCollider2D> ();
        sr = GetComponent<SpriteRenderer>();
        previousPos = rb.position.y;
    }

    // Called at a fixed rate
    void FixedUpdate()
    {
        float movement = Input.GetAxisRaw("Vertical") * Time.deltaTime * paddleSpeed;
        Vector2 newPosition = new(rb.position.x, rb.position.y + movement);
        float minY = Bounds.BottomPos.y + bc.size.y;
        float maxY = Bounds.TopPos.y - bc.size.y;
        yRange = Mathf.Clamp(newPosition.y, minY, maxY);
        Vector2 clampedPos = new(rb.position.x, yRange);
        
        rb.position = clampedPos;
        Invoke(nameof(PaddleVelocity), timeInterval);
        storedPaddleVelocity = paddleVelocity;
        
    }

    //
    void PaddleVelocity()
    {
        float currentPos = yRange;
        deltaPos = currentPos - previousPos;
        paddleVelocity = deltaPos/timeInterval;
        
        previousPos = currentPos;
    }

    
}
