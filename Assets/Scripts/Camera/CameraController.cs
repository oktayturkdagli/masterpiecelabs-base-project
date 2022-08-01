using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform camTransform;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;
        
        public Transform Target { get => target; set => target = value; }
        public Transform CamTransform { get => camTransform; set => camTransform = value; }
        public Vector3 Offset { get => offset; set => offset = value; }
        public float SmoothTime { get => smoothTime; set => smoothTime = value; }
        
        private void Start()
        {
            offset = camTransform.position - target.position;
        }

        private void LateUpdate()
        {
            var targetPosition = target.position + offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}