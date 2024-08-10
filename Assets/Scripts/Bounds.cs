using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PongUnity.CoreConstants;

public class Bounds : MonoBehaviour
{
    // Start is called before the first frame update
    private const float OriginX = 0f; 
    private const float OriginY = 0f;
    public struct VerticalBounds
    {
        private float min;
        private float max;

        public float Min { get => min; set => min = value; }
        public float Max { get => max; set => max = value; }
    };
    private static readonly float _boundsWidth = 0.3f;
    private static GameObject bounds, top, bottom, left, right;
    private BoxCollider2D topBox, bottomBox, leftBox, rightBox;
    private static VerticalBounds yLimiter;
    private protected static Camera cam;
    public static VerticalBounds YLimiter
    {
        get 
        {
            return yLimiter;
        }
    }
    public static Vector2 TopPos { get => top.transform.position;}
    public static Vector2 BottomPos { get => bottom.transform.position;}
    public static Vector2 LeftPos { get => left.transform.position;}
    public static Vector2 RightPos {get => right.transform.position;}
    public static float BoundsWidth { get => _boundsWidth; }
    void Awake()
    {
        bounds = gameObject;
        cam = GetComponentInParent<Camera>();
        bounds.transform.position =  cam.transform.position;
        CreateBarriers();
    }

    internal class CamBounds
    {
        #region Fields
        
        // Gets the bounds of the cam viewport and converts them to worldspace coords
        private static readonly Vector3 _topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane));
        private static readonly Vector3 _bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
        private static readonly Vector3 _vpCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        private static readonly float _vpWidth = _topRight.x - _bottomLeft.x;
        private static readonly float _vpHeight = _topRight.y - _bottomLeft.y;

        #endregion

        #region Properties

        // For getting the bounds fields
        public static Vector3 TopRight { get => _topRight; }
        public static Vector3 BottomLeft { get => _bottomLeft; }
        public static Vector3 VpCenter { get => _vpCenter; }
        public static float VpWidth { get => _vpWidth; }
        public static float VpHeight { get => _vpHeight; }

        #endregion     
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBarriers()
    {
        //create objects
        top = new(Names.Top);
        bottom = new(Names.Bottom);
        left = new(Names.Left);
        right = new(Names.Right);
        
        //set tags for the objects
        top.tag = Tags.WallTag;
        bottom.tag = Tags.WallTag;
        left.tag = Tags.P1GoalTag;
        right.tag = Tags.P2GoalTag;

        // Make bounds object the parent
        top.transform.SetParent(bounds.transform, false);
        bottom.transform.SetParent(bounds.transform, false);
        left.transform.SetParent(bounds.transform, false);
        right.transform.SetParent(bounds.transform, false); 
        
        // Position the barriers top and bottom
        Vector3 topPos = new(CamBounds.VpCenter.x, CamBounds.TopRight.y, cam.nearClipPlane);
        Vector3 bottomPos = new(CamBounds.VpCenter.x, CamBounds.BottomLeft.y, cam.nearClipPlane);
        Vector3 leftPos = new(CamBounds.BottomLeft.x, CamBounds.VpCenter.y, cam.nearClipPlane);
        Vector3 rightPos = new(CamBounds.TopRight.x, CamBounds.VpCenter.x, cam.nearClipPlane);

        // Store this position for range limiters of paddles
        yLimiter.Max = topPos.y - _boundsWidth;
        yLimiter.Min = bottomPos.y + _boundsWidth; 

        top.transform.position = topPos;
        bottom.transform.position = bottomPos;
        left.transform.position = leftPos;
        right.transform.position = rightPos;
        
        // Create box colliders
        topBox = top.AddComponent<BoxCollider2D> ();
        bottomBox = bottom.AddComponent<BoxCollider2D> ();
        leftBox = left.AddComponent<BoxCollider2D> ();
        rightBox = right.AddComponent<BoxCollider2D> ();
        
        //Declare the sizes of the boxes
        topBox.size = new Vector2(CamBounds.VpWidth, _boundsWidth);
        bottomBox.size = new Vector2(CamBounds.VpWidth, _boundsWidth);
        leftBox.size = new Vector2(_boundsWidth, CamBounds.VpHeight);
        rightBox.size = new Vector2(_boundsWidth, CamBounds.VpHeight);
        
        top.tag = Tags.WallTag;
        bottom.tag = Tags.WallTag;
        left.tag = Tags.P1GoalTag;
        right.tag = Tags.P2GoalTag;

        //These will be the goals
        topBox.isTrigger = true;
        bottomBox.isTrigger = true;
        leftBox.isTrigger = true;
        rightBox.isTrigger = true;

    }
}
