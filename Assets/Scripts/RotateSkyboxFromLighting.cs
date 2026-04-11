using UnityEngine;

public class RotateSkyboxFromLighting : MonoBehaviour
{
    public float speed = 1f;
    private Material skyboxMat;
    float rotation;

    void Start()
    {
        skyboxMat = RenderSettings.skybox;
    }

    void Update()
    {
        if (skyboxMat == null) return;

        rotation += speed * Time.deltaTime;
        if (rotation >= 360f) rotation = 0f;

        skyboxMat.SetFloat("_Rotation", rotation);
    }
}