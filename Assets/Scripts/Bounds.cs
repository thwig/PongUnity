using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera cam;
    [SerializeField] private float boundsWidth = 1f;
    GameObject bounds, top, bottom, left, right;
    BoxCollider2D topBox, bottomBox, leftBox, rightBox;

    void Start()
    {
        bounds = gameObject;
        cam = GetComponentInParent<Camera>();
        bounds.transform.position =  cam.transform.position;
        CreateBarriers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBarriers()
    {
        //create objects
        top = new ("Top");
        bottom = new GameObject("Bottom");
        left = new GameObject("Left");
        right = new GameObject("Right");
        
        //set tags for the objects
        top.tag = Tags.WallTag;
        bottom.tag = Tags.WallTag;
        left.tag = Tags.P1GoalTag;
        right.tag = Tags.P2GoalTag;

        //camera dimensions
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane));
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
        Vector3 vpCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        float vpWidth = topRight.x - bottomLeft.x;
        float vpHeight = topRight.y - bottomLeft.y;
        
        //make bounds object the parent
        top.transform.SetParent(bounds.transform, false);
        bottom.transform.SetParent(bounds.transform, false);
        left.transform.SetParent(bounds.transform, false);
        right.transform.SetParent(bounds.transform, false); 
        
        //position the barriers top and bottom
        Vector3 topPos = new(vpCenter.x, topRight.y, cam.nearClipPlane);
        Vector3 bottomPos = new(vpCenter.x, bottomLeft.y, cam.nearClipPlane);
        Vector3 leftPos = new(bottomLeft.x, vpCenter.y, cam.nearClipPlane);
        Vector3 rightPos = new(topRight.x, vpCenter.x, cam.nearClipPlane);

        top.transform.position = topPos;
        bottom.transform.position = bottomPos;
        left.transform.position = leftPos;
        right.transform.position = rightPos;
        
        //create box colliders
        topBox = top.AddComponent<BoxCollider2D> ();
        bottomBox = bottom.AddComponent<BoxCollider2D> ();
        leftBox = left.AddComponent<BoxCollider2D> ();
        rightBox = right.AddComponent<BoxCollider2D> ();

        //Declare the sizes of the boxes
        topBox.size = new Vector2(vpWidth, boundsWidth);
        bottomBox.size = new Vector2(vpWidth, boundsWidth);
        leftBox.size = new Vector2(boundsWidth, vpHeight);
        rightBox.size = new Vector2(boundsWidth, vpHeight);

        //These will be the goals
        topBox.isTrigger = true;
        bottomBox.isTrigger = true;
        leftBox.isTrigger = true;
        rightBox.isTrigger = true;

    }
}
