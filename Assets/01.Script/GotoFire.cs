using System;
using TMPro;
using UnityEngine;

namespace _01.Script
{
    public class GotoFire : MonoBehaviour
    {
        [SerializeField] private RectTransform arrowUI;      
        [SerializeField] private RectTransform fireUI;
        [SerializeField] private Transform player;           
        [SerializeField] private Transform target;           
        [SerializeField] private Camera cam;
        [SerializeField] private float borderPadding = 50f;
        [SerializeField] private float ActiveDistance = 15f;
        private Quaternion initialRotation;

        private void Awake()
        {
            initialRotation = fireUI.rotation;
        }

        private void Update()
        {
            if (target == null || player == null) return;

            Vector3 playerToTarget = target.position - player.position;
            float distance = playerToTarget.magnitude;
    
            if (distance <= ActiveDistance)
            {
                arrowUI.gameObject.SetActive(false);
                return;
            }

            Vector3 screenPos = cam.WorldToScreenPoint(target.position);
            bool isBehind = screenPos.z < 0;

            if (isBehind)
            {
                screenPos.x = -screenPos.x;
                screenPos.y = -screenPos.y;
            }

            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 screenPos2D = new Vector2(screenPos.x, screenPos.y);
            Vector2 dir = (screenPos2D - screenCenter).normalized;
    
            bool isOffScreen = isBehind || screenPos.x < 0 || screenPos.x > Screen.width || 
                               screenPos.y < 0 || screenPos.y > Screen.height;

            Vector2 finalPosition = screenPos2D;
            if (isOffScreen)
            {
                finalPosition = screenCenter + dir * 1000f;
                finalPosition.x = Mathf.Clamp(finalPosition.x, borderPadding, Screen.width - borderPadding);
                finalPosition.y = Mathf.Clamp(finalPosition.y, borderPadding, Screen.height - borderPadding);
            }

            arrowUI.gameObject.SetActive(true);
            fireUI.rotation = initialRotation;
            arrowUI.position = finalPosition;
            arrowUI.up = dir;
        }
    }
}
