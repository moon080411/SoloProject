using UnityEngine;
using UnityEngine.UI;

namespace _01.Script
{
    public class UIScaleSetting : MonoBehaviour
    {
        private void Awake()
        {
            CanvasScaler scaler = GetComponent<CanvasScaler>();

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2560, 1440);
        
            float screenAspect = (float)Screen.width / Screen.height;
            float targetAspect = 16f / 9f;
        
            scaler.matchWidthOrHeight = (screenAspect < targetAspect) ? 0f : 1f;
        }
    }
}
