using UnityEngine;

namespace Player
{
    public sealed class CameraFollower : MonoBehaviour
    {
        private const int CAMERA_DISTANCE = -10;
        
        [SerializeField] private Transform followTarget;

        private void LateUpdate() => transform.position = new (followTarget.position.x, followTarget.position.y, CAMERA_DISTANCE);
    }
}