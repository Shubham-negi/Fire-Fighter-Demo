using HurricaneVR.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class dragdrop3d : MonoBehaviour
{
    [Header("Target Settings")]
    public Collider Target; 

    [Header("Linked Objects")]
    public GameObject[] truObj, falseObj, delayObj, colObj;

    [Header("Events")]
    public UnityEvent OnAllPlacementComplete; 
    [Header("HVR Events")]
    public UnityEvent HVRActive; 
    private static int totalTargets = 4;
    private static int placedCount = 0;

    private bool isPlaced = false;          

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (other.gameObject == Target.gameObject)
        {
            isPlaced = true;

            HVRGrabbable hvr = other.GetComponent<HVRGrabbable>();
            if (hvr)
            {
                hvr.enabled = false;
            }
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.SetActive(false);
            gameObject.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            //other.transform.localPosition = transform.localPosition;
            //other.transform.localRotation = transform.localRotation;
            //other.transform.localScale = transform.localScale;


            NextMsg();
            Debug.Log("✅ Cube snapped to dragdrop object position & grab disabled.");

            placedCount++;

            Debug.Log("Drop Count : " + placedCount);

            if (placedCount == totalTargets)
            {
                Debug.Log("🎉 All cubes placed successfully!");
                OnAllPlacementComplete?.Invoke();
            }
        }
    }

    void NextMsg()
    {
        foreach (var obj in truObj) obj.SetActive(true);

        foreach (var obj in falseObj) obj.SetActive(false);
        foreach (var obj in delayObj) obj.SetActive(false);

        foreach (var obj in colObj)
        {
            BoxCollider col = obj.GetComponent<BoxCollider>();
            if (col != null) col.enabled = true;
        }

        HVRActive?.Invoke();

      //  gameObject.SetActive(false);
    }
}
