using UnityEngine;

namespace PongUnity.CoreConstants
{
    public class Tags : MonoBehaviour
    {
        public const string WallTag = "Wall";
        public const string P1GoalTag = "Player1Goal";
        public const string P2GoalTag = "Player2Goal";
        public const string PaddleTag = "Paddle";
        public const string NetTag = "Net";
    }
    public class Names : MonoBehaviour
    {
        public const string MainCamera = "Main Camera", Top = "Top", Bottom = "Bottom",
        Left = "Left", Right = "Right";
        
        public const string BallName = "Ball";
        
        
    }

    public class NumericConstants : MonoBehaviour
    {
        public const float OriginX = 0f;
        public const float OriginY = 0f;
        public const float OriginZ = 0f;
    }

    public class Layers : MonoBehaviour
    {
        public const string CollisionLayer = "Collision Layer";
        public const int IgnoreRayCast = 2;
        public const int Default = 0;
    }
}
