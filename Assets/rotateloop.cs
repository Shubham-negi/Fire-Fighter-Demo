using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public float rotationSpeed = 360f; // degrees per second

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}