using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float difficulty;
    [SerializeField] float range;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float target = Mathf.Lerp(rb.velocity.y, Predict(), 0.1f);
        float minY = -range;
        float maxY = range;
        float currentY = rb.position.y;

        if (currentY <= minY && target < 0) target = 0;
        if (currentY >= maxY && target > 0) target = 0;

        rb.velocity = new Vector2(rb.velocity.x, target);
        rb.velocity *= difficulty;
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, -range, range);
        rb.position = clampedPos;

         Debug.Log($"Target Velocity: {target}, Current Y: {currentY}, Predicted Y: {Predict()}");
    }

    float Predict()
    {
        return BallController.StoredVelocity.y * 1.2f / BallController.StoredVelocity.x * 1.2f;
    }
}
