using UnityEngine;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "CamSetting", menuName = "SO/CamSetting")]
    public class CamSettingSO : ScriptableObject
    {
        
        [field: SerializeField]
        public float TurnSpeedX { get; private set; } = 5;
    
        [field: SerializeField]
        public float TurnSpeedY { get; private set; } = 2.5f;
        
        
        [field: SerializeField]
        public Vector2 XMinMax {get; private set;} = new Vector2(-53f, 72f);
    
        [field: SerializeField]
        public Vector2 CamDistanceMinMax {get; private set;} = new Vector2(0.01f, 10f);
        
        [field:SerializeField]
        public Vector3 CamDefaultRotate { get; private set; }
    }
}
