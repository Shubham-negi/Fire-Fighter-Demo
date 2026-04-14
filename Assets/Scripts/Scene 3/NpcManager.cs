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
        foreach (var data in npcs)
        {
            if (data.isMoving && data.npc != null)
            {
                MoveNPC(data);
            }
        }
    }

    // 🔹 Call specific NPC
[ContextMenu("Call ALL NPCs")]
public void CallAllNpcs()
{
    for (int i = 0; i < npcs.Count; i++)
    {
        CallNpc(i);
    }
}    public void CallNpc(int index)
    {
        if (index < 0 || index >= npcs.Count) return;

        var data = npcs[index];

        if (data.npc == null || data.waypoints.Count == 0) return;

        data.currentIndex = 0;
        data.isMoving = true;

        // ▶️ Run Animation
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

        Vector3 targetPos = target.position;
        targetPos.y = data.npc.position.y;

        Vector3 direction = (targetPos - data.npc.position).normalized;

        // Move
        data.npc.position += direction * moveSpeed * Time.deltaTime;

        // Rotate
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            data.npc.rotation = Quaternion.Slerp(data.npc.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        float distance = Vector3.Distance(data.npc.position, targetPos);

        if (distance <= reachDistance)
        {
            // ✅ Adjust only Y
            Vector3 pos = data.npc.position;
            pos.y = target.position.y;
            data.npc.position = pos;

            data.currentIndex++;

            if (data.currentIndex >= data.waypoints.Count)
            {
                data.isMoving = false;

                // ▶️ Idle Animation
                if (data.idleState != null)
                {
                    data.idleState.wrapMode = WrapMode.Loop;
                    data.anim.Play(data.idleState.name);
                }
            }
        }
    }
}