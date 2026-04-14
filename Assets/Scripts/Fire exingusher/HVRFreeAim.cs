using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using UnityEngine;

namespace HurricaneVR.Framework.Core
{
    [RequireComponent(typeof(HVRGrabbable))]
    public class HVRFreeAim : MonoBehaviour
    {
        public bool FollowRotation = true;

        private const float MinLocalY = -0.5f;

        private HVRGrabbable _grabbable;
        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;

        private void Awake()
        {
            _grabbable = GetComponent<HVRGrabbable>();
            _grabbable.HandGrabbed.AddListener(OnGrabbed);
        }

        private void OnDestroy()
        {
            _grabbable.HandGrabbed.RemoveListener(OnGrabbed);
        }

        private void OnGrabbed(HVRHandGrabber grabber, HVRGrabbable grabbable)
        {
            Transform handTransform = grabber.transform;

            _positionOffset = Quaternion.Inverse(handTransform.rotation) *
                              (transform.position - handTransform.position);

            _rotationOffset = Quaternion.Inverse(handTransform.rotation) *
                              transform.rotation;
        }

        private void LateUpdate()
        {
            if (!_grabbable.IsBeingHeld)
                return;

            if (!(_grabbable.PrimaryGrabber is HVRHandGrabber hand))
                return;

            Transform handTransform = hand.transform;

            // World position calculation
            Vector3 worldTargetPosition = handTransform.position +
                                          handTransform.rotation * _positionOffset;

            transform.position = worldTargetPosition;

            // 🔒 HARD LOCAL Y CLAMP (cannot go below -0.5)
            Vector3 localPos = transform.localPosition;
            if (localPos.y < MinLocalY)
            {
                localPos.y = MinLocalY;
                transform.localPosition = localPos;
            }

            if (FollowRotation)
            {
                transform.rotation = handTransform.rotation * _rotationOffset;
            }
        }
    }
}
