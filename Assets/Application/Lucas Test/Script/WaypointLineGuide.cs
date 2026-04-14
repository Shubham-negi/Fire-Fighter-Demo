using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class WaypointLineGuide : MonoBehaviour
{
    public Transform player;

    [Header("Path Setup")]
    public List<Transform> waypoints = new List<Transform>();

    public float heightOffset = 0.05f;
    public float scrollSpeed = 1f;

    [Header("Rotation Lock")]
    public bool lockRotation = true;
    public Vector3 lockedEulerAngles = new Vector3(90f, 0f, 0f);

    private LineRenderer line;
    private Material mat;
    private float offset;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        mat = line.material;

        line.useWorldSpace = true;
        line.alignment = LineAlignment.TransformZ;

        // Auto assign parent if not set

    }

    void LateUpdate()
    {
        if (lockRotation)
            transform.rotation = Quaternion.Euler(lockedEulerAngles);

        if (player == null)
        {
            line.positionCount = 0;
            return;
        }

        DrawPath();
        Animate();
    }

    void DrawPath()
    {
        List<Vector3> rawPoints = new List<Vector3>();

        // 🔹 Player
        Vector3 start = player.position;
        start.y += heightOffset;
        rawPoints.Add(start);

        // 🔹 Waypoints
        foreach (var wp in waypoints)
        {
            if (wp == null) continue;

            Vector3 p = wp.position;
            p.y += heightOffset;
            rawPoints.Add(p);
        }

        // 🔹 Target
        Vector3 end = transform.position;
        end.y += heightOffset;
        rawPoints.Add(end);

        // 🔥 CURVED SMOOTHING (Catmull-Rom)
        List<Vector3> smoothPoints = new List<Vector3>();

        int resolution = 8; // increase = smoother

        for (int i = 0; i < rawPoints.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? rawPoints[i] : rawPoints[i - 1];
            Vector3 p1 = rawPoints[i];
            Vector3 p2 = rawPoints[i + 1];
            Vector3 p3 = (i + 2 < rawPoints.Count) ? rawPoints[i + 2] : p2;

            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;

                Vector3 point =
                    0.5f * (
                    (2f * p1) +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
                    );

                smoothPoints.Add(point);
            }
        }

        // Add last point
        smoothPoints.Add(rawPoints[rawPoints.Count - 1]);

        line.positionCount = smoothPoints.Count;

        for (int i = 0; i < smoothPoints.Count; i++)
        {
            line.SetPosition(i, smoothPoints[i]);
        }
    }
    void Animate()
    {
        if (mat == null || line.positionCount == 0) return;

        offset -= Time.deltaTime * scrollSpeed;
        mat.SetFloat("_Offset", offset);
    }
}