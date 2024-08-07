using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float difficulty;
    [SerializeField] float range;

    Rigidbody2D rb;
    private float restingPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        restingPos = rb.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float minY = -range;
        float maxY = range;
        float currentY = rb.position.y;
        float target = BallController.StoredPosition.y * 2;       

        if (BallController.StoredPosition.x >= 0) Exterpolate(target);       
        else Exterpolate(restingPos);
        
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }

    void Exterpolate (float target)
    {
        // Finds a target point
        Debug.Log($"Target: {target}");
        if (rb.position.y != target) rb.velocity = new Vector2 (rb.velocity.x, target);   
        return;
    }
}
