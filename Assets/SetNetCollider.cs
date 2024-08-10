using PongUnity.CoreConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNetCollider : MonoBehaviour
{
    // Start is called before the first frame update
    EdgeCollider2D edgeCollider;
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points[0] = new Vector2(NumericConstants.OriginX, Bounds.TopPos.y);
        edgeCollider.points[1] = new Vector2(NumericConstants.OriginX, Bounds.BottomPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
