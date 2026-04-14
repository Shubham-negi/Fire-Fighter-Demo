using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class WaypointAnimatedPath : MonoBehaviour
{
    [Header("Auto Points (Move in Scene)")]
    public int pointCount = 5;
    public float spacing = 2f;

    [Header("Line Settings")]
    public float heightOffset = 0.05f;

    [Header("Animation")]
    public float scrollSpeed = 2f;

    private List<Transform> points = new List<Transform>();
    private LineRenderer lr;
    private Material mat;
    private float offset;

    void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        mat = lr.material;

        GeneratePoints();
    }

    void OnValidate()
    {
        GeneratePoints();
    }

    void GeneratePoints()
    {
        // Clear old points
        while (transform.childCount > 0)
        {
            if (Application.isEditor)
                DestroyImmediate(transform.GetChild(0).gameObject);
            else
                Destroy(transform.GetChild(0).gameObject);
        }

        points.Clear();

        // Create new editable points
        for (int i = 0; i < pointCount; i++)
        {
            GameObject p = new GameObject("Point_" + i);
            p.transform.parent = transform;

            p.transform.localPosition = new Vector3(i * spacing, 0, 0);

            points.Add(p.transform);
        }
    }

    void Update()
    {
        if (points.Count == 0 || lr == null) return;

        // 🔹 Draw line through points
        lr.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 pos = points[i].position;
            pos.y += heightOffset;

            lr.SetPosition(i, pos);
        }

        // 🔹 Animate texture (arrow flow)
        if (mat != null)
        {
            offset += Time.deltaTime * scrollSpeed;
            mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}