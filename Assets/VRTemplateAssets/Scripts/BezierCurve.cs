using System;
using UnityEngine;

namespace Unity.VRTemplate
{
    public class BezierCurve : MonoBehaviour
    {
        public enum UpdateType
        {
            UpdateAndBeforeRender,
            Update,
            BeforeRender,
        }

        [Header("Update")]
        [SerializeField] UpdateType m_UpdateTrackingType = UpdateType.Update;

        [Header("Points")]
        [SerializeField] Transform m_StartPoint;
        [SerializeField] Transform m_MidPoint1;
        [SerializeField] Transform m_MidPoint2;
        [SerializeField] Transform m_EndPoint;

        [Header("Curve")]
        [SerializeField] int m_SegmentCount = 50;

        [Header("Line")]
        [SerializeField] LineRenderer m_LineRenderer;

        Vector3 m_LastStartPosition;
        Vector3 m_LastEndPosition;

        void Awake()
        {
            if (m_LineRenderer == null)
                m_LineRenderer = GetComponent<LineRenderer>();
        }

        void OnEnable()
        {
            DrawCurve();
            Application.onBeforeRender += OnBeforeRender;
        }

        void OnDisable()
        {
            Application.onBeforeRender -= OnBeforeRender;
        }

        void OnBeforeRender()
        {
            if (m_UpdateTrackingType == UpdateType.BeforeRender || m_UpdateTrackingType == UpdateType.UpdateAndBeforeRender)
                DrawCurve();
        }

        void Update()
        {
            if (m_UpdateTrackingType == UpdateType.Update || m_UpdateTrackingType == UpdateType.UpdateAndBeforeRender)
                DrawCurve();
        }

        public void DrawCurve()
        {
            if (m_StartPoint == null || m_EndPoint == null || m_MidPoint1 == null || m_MidPoint2 == null)
                return;

            var p0 = m_StartPoint.position;
            var p1 = m_MidPoint1.position;
            var p2 = m_MidPoint2.position;
            var p3 = m_EndPoint.position;

            if (p0 == m_LastStartPosition && p3 == m_LastEndPosition)
                return;

            m_LineRenderer.positionCount = m_SegmentCount + 1;

            for (int i = 0; i <= m_SegmentCount; i++)
            {
                float t = i / (float)m_SegmentCount;
                Vector3 point = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
                m_LineRenderer.SetPosition(i, point);
            }

            m_LastStartPosition = p0;
            m_LastEndPosition = p3;
        }

        static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return (uu * u * p0) +
                   (3 * uu * t * p1) +
                   (3 * u * tt * p2) +
                   (tt * t * p3);
        }
    }
}