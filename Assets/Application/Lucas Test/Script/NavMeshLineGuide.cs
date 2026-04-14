using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavMeshLineGuide : MonoBehaviour
{
    public Transform player;          // XR player root
    public Transform target;          // usually the Destination Point root

    public float heightOffset = 0.05f;
    public float scrollSpeed = 1f;

    [Header("Rotation Lock")]
    public bool lockRotation = true;
    public Vector3 lockedEulerAngles = new Vector3(90f, 0f, 0f); // flat on floor

    private LineRenderer line;
    private NavMeshPath path;
    private Material mat;
    private float offset;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        mat = line.material;

        line.useWorldSpace = true;
        line.alignment = LineAlignment.TransformZ;

        // if target not set in inspector, use parent (Destination Point)
        if (target == null && transform.parent != null)
            target = transform.parent;
    }

    void LateUpdate()
    {
        if (lockRotation)
            transform.rotation = Quaternion.Euler(lockedEulerAngles);

        if (player == null || target == null)
        {
            line.positionCount = 0;
            return;
        }

        DrawPath();
        Animate();
    }

    void DrawPath()
    {
        if (!NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path))
        {
            line.positionCount = 0;
            return;
        }

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            line.positionCount = 0;
            return;
        }

        var corners = path.corners;
        if (corners.Length < 2)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 p = corners[i];
            p.y += heightOffset;
            line.SetPosition(i, p);
        }
    }

    void Animate()
    {
        if (mat == null || line.positionCount == 0) return;

        offset += Time.deltaTime * scrollSpeed;
        mat.SetFloat("_Offset", offset); // property from your arrow shader
    }
}
