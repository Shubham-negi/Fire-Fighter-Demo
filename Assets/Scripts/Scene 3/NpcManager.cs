using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData
{
    public Transform npc;

    [HideInInspector] public Animation anim;
    [HideInInspector] public AnimationState runState;
    [HideInInspector] public AnimationState idleState;

    public List<Transform> waypoints = new List<Transform>();

    [HideInInspector] public int currentIndex = 0;
    [HideInInspector] public bool isMoving = false;
}

public class NpcManager : MonoBehaviour
{
    [Header("All NPCs")]
    public List<NPCData> npcs = new List<NPCData>();

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float reachDistance = 0.2f;

    // 🔥 NOZZLES
    [Header("Nozzles")]
    public List<Transform> nozzles = new List<Transform>();

    [Header("Nozzle Rotation")]
    public Vector3 minRotation;
    public Vector3 maxRotation;
    public float nozzleSpeed = 1f;

    private bool animateNozzles = false;
    private float nozzleTime = 0f;

    // 🔥 WATER + FOAM
    [Header("Water & Foam Objects")]
    public List<GameObject> waterObjects = new List<GameObject>();
    public List<GameObject> foamObjects = new List<GameObject>();

    void Start()
    {
        foreach (var data in npcs)
        {
            if (data.npc == null) continue;

            data.anim = data.npc.GetComponent<Animation>();

            int i = 0;
            foreach (AnimationState state in data.anim)
            {
                if (i == 4) data.runState = state;   // run forward
                if (i == 3) data.idleState = state;  // looking around
                i++;
            }

            data.anim.Stop();
        }
    }

    void Update()
    {
        // 🔹 NPC movement
        foreach (var data in npcs)
        {
            if (data.isMoving && data.npc != null)
            {
                MoveNPC(data);
            }
        }

        // 🔹 Nozzle animation
        if (animateNozzles)
        {
            AnimateNozzles();
        }
    }

    // 🔹 Call all NPCs
    [ContextMenu("Call ALL NPCs")]
    public void CallAllNpcs()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            CallNpc(i);
        }
    }

    public void CallNpc(int index)
    {
        if (index < 0 || index >= npcs.Count) return;

        var data = npcs[index];

        if (data.npc == null || data.waypoints.Count == 0) return;

        data.currentIndex = 0;
        data.isMoving = true;

        // ▶️ Run animation
        if (data.runState != null)
        {
            data.runState.wrapMode = WrapMode.Loop;
            data.anim.Play(data.runState.name);
        }

        // Face first waypoint
        Vector3 dir = (data.waypoints[0].position - data.npc.position).normalized;
        if (dir != Vector3.zero)
            data.npc.rotation = Quaternion.LookRotation(dir);
    }

    void MoveNPC(NPCData data)
    {
        Transform target = data.waypoints[data.currentIndex];

        // 🔹 Lock Y while moving
        Vector3 targetPos = target.position;
        targetPos.y = data.npc.position.y;

        Vector3 direction = (targetPos - data.npc.position).normalized;

        // 🔹 Move
        data.npc.position += direction * moveSpeed * Time.deltaTime;

        // 🔹 Rotate
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            data.npc.rotation = Quaternion.Slerp(data.npc.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        float distance = Vector3.Distance(data.npc.position, targetPos);

        if (distance <= reachDistance)
        {
            // ✅ Adjust only Y at destination
            Vector3 pos = data.npc.position;
            pos.y = target.position.y;
            data.npc.position = pos;

            data.currentIndex++;

            if (data.currentIndex >= data.waypoints.Count)
            {
                data.isMoving = false;

                // ▶️ Idle animation
                if (data.idleState != null)
                {
                    data.idleState.wrapMode = WrapMode.Loop;
                    data.anim.Play(data.idleState.name);
                }

                // 🔥 Start nozzle animation
                animateNozzles = true;

                // 🔥 Activate water
                foreach (var water in waterObjects)
                {
                    if (water != null)
                        water.SetActive(true);
                }

                // 🔥 Activate foam
                foreach (var foam in foamObjects)
                {
                    if (foam != null)
                        foam.SetActive(true);
                }
            }
        }
    }

    void AnimateNozzles()
    {
        nozzleTime += Time.deltaTime * nozzleSpeed;

        float t = Mathf.PingPong(nozzleTime, 1f);

        Vector3 rot = Vector3.Lerp(minRotation, maxRotation, t);

        foreach (var nozzle in nozzles)
        {
            if (nozzle != null)
            {
                nozzle.localRotation = Quaternion.Euler(rot);
            }
        }
    }
}