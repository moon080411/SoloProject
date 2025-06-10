using UnityEngine;

namespace _01.Script
{
    public class Billboard : MonoBehaviour
    {
        Camera _cam;

        void Start()
        {
            _cam = Camera.main;
        }

        void FixedUpdate()
        {
            Vector3 dir = transform.position - _cam.transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
