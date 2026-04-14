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

    // ✅ SINGLE DESTINATION
    [Header("Destination")]
    public Transform destination;

    // 🔥 NOZZLE
    [Header("Nozzle")]
    public Transform nozzle;
    public Vector3 minRotation;
    public Vector3 maxRotation;
    public float nozzleSpeed = 1f;

    // 🔥 SINGLE LIQUID SOURCE
    [Header("Liquid Source")]
    public GameObject liquidSource;

    // Runtime
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool reached = false;

    [HideInInspector] public float nozzleTime = 0f;
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
                if (i == 4) data.runState = state;
                if (i == 2) data.idleState = state;
                i++;
            }

            data.anim.Stop();

            // 🔹 Make sure liquid is OFF initially
            if (data.liquidSource != null)
                data.liquidSource.SetActive(false);
        }
    }

    void Update()
    {
        foreach (var data in npcs)
        {
            if (data.isMoving && !data.reached)
            {
                MoveNPC(data);
            }

            if (data.reached)
            {
                AnimateNozzle(data);
            }
        }
    }

    [ContextMenu("Call ALL NPCs")]
    public void CallAllNpcs()
    {
        foreach (var data in npcs)
        {
            CallNpc(data);
        }
    }

    void CallNpc(NPCData data)
    {
        if (data.npc == null || data.destination == null) return;

        data.isMoving = true;
        data.reached = false;

        if (data.runState != null)
        {
            data.runState.wrapMode = WrapMode.Loop;
            data.anim.Play(data.runState.name);
        }

        Vector3 dir = (data.destination.position - data.npc.position).normalized;
        if (dir != Vector3.zero)
            data.npc.rotation = Quaternion.LookRotation(dir);
    }

    void MoveNPC(NPCData data)
    {
        Vector3 targetPos = data.destination.position;
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
            // ✅ Adjust only Y
            Vector3 pos = data.npc.position;
            pos.y = data.destination.position.y;
            data.npc.position = pos;

            data.isMoving = false;
            data.reached = true;

            // ▶️ Idle
            if (data.idleState != null)
            {
                data.idleState.wrapMode = WrapMode.Loop;
                data.anim.Play(data.idleState.name);
            }

            // 🔥 Activate liquid
            if (data.liquidSource != null)
                data.liquidSource.SetActive(true);
        }
    }

    void AnimateNozzle(NPCData data)
    {
        if (data.nozzle == null) return;

        data.nozzleTime += Time.deltaTime * data.nozzleSpeed;

        float t = Mathf.PingPong(data.nozzleTime, 1f);

        Vector3 rot = Vector3.Lerp(data.minRotation, data.maxRotation, t);

        data.nozzle.localRotation = Quaternion.Euler(rot);
    }
}