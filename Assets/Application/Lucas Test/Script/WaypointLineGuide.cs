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
        int count = 1 + waypoints.Count + 1; // Player + waypoints + target
        line.positionCount = count;

        int index = 0;

        // 🔹 1. Player (START)
        Vector3 p = player.position;
        p.y += heightOffset;
        line.SetPosition(index++, p);

        // 🔹 2. Waypoints (MIDDLE)
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            Vector3 wp = waypoints[i].position;
            wp.y += heightOffset;
            line.SetPosition(index++, wp);
        }

        // 🔹 3. Target (END)
        Vector3 t = transform.position;
        t.y += heightOffset;
        line.SetPosition(index++, t);

        // Fix count if some waypoints were null
        line.positionCount = index;
    }

    void Animate()
    {
        if (mat == null || line.positionCount == 0) return;

        offset += Time.deltaTime * scrollSpeed;
        mat.SetFloat("_Offset", offset);
    }
}