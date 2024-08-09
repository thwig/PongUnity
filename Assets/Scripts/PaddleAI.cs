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
   
    void Start()
    {
        Paddles.InitializePaddlePosition(gameObject.transform);
        rb = GetComponent<Rigidbody2D>();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    
}
