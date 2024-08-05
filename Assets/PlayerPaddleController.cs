using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float paddleSpeed;
    [SerializeField] float range;

    private static Vector2 storedVelocity;
    Rigidbody2D rb;
    BoxCollider2D bc;

    public static Vector2 StoredVelocity
    {
        get { return storedVelocity; }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        bc = GetComponent<BoxCollider2D> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = Input.GetAxis("Vertical");
        float newVelY = movement * paddleSpeed;
        float minY = -range;
        float maxY = range;
        float currentY = rb.position.y;

        if (currentY <= minY && newVelY < 0) newVelY = 0;
        if (currentY >= maxY && newVelY > 0) newVelY = 0;

        rb.velocity = new Vector2(rb.velocity.x, newVelY);
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, -range, range);
        rb.position = clampedPos;

        storedVelocity = rb.velocity;
    }
}
