using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float difficulty;
    [SerializeField] float range;
    [SerializeField] float predicterTweak;
    [SerializeField] GameObject ball;
    Rigidbody2D rb;
    private float restingPos;
    GameObject ghostBall;
    SpriteRenderer invisible;
    Rigidbody2D predictedPath;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MakeGhostBall();
        restingPos = rb.position.y;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float minY = -range;
        float maxY = range;
        float currentY = rb.position.y;
        float target = 0;       

        predictedPath.velocity = new Vector2(BallController.StoredVelocity.x + predicterTweak, BallController.StoredVelocity.y + predicterTweak);
        if (predictedPath.position.x == rb.position.x) 
        {
            target = predictedPath.position.y;
            Destroy(ghostBall);
            MakeGhostBall();
            Debug.Log("Ghost collided with x");
        }

        if (BallController.StoredPosition.x >= 0) Exterpolate(target);       
        //else RestingExterpolate(restingPos);
        
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }
    void MakeGhostBall()
    {
        ghostBall = Instantiate(ball);
        invisible = ghostBall.GetComponent<SpriteRenderer>();
        predictedPath = ghostBall.GetComponent<Rigidbody2D>();
        invisible.enabled = true;
        invisible.color = Color.red;    
    }
    void Exterpolate (float target)
    {
        // Finds a target point
        //Debug.Log($"Target: {target}");
        float newVelocity = difficulty * Mathf.Sign(target);
        if (rb.position.y != target)
        {
             rb.velocity = new Vector2 (rb.velocity.x, newVelocity);  
        } 
        return;
    }
   /* void RestingExterpolate (float target)
    {
        // Finds a target point
        Debug.Log($"Target: {target}");
        if (rb.position.y != target) 
        {
            float newVelocity = difficulty * -Mathf.Sign(rb.position.y);
            rb.velocity = new Vector2 (rb.velocity.x, newVelocity);   
        }
    } */
}
