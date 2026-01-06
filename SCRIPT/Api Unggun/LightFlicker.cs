using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.9f;
    public float maxIntensity = 1.6f;
    public float flickerSpeed = 10f; // seberapa cepat berubah
    public float colorVariation = 0.05f; // sedikit variasi warna

    private Light lt;
    private Color baseColor;

    void Awake()
    {
        lt = GetComponent<Light>();
        baseColor = lt.color;
    }

    void Update()
    {
        float t = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        lt.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        // sedikit variasi warna (opsional)
        float colOffset = (t - 0.5f) * colorVariation;
        lt.color = baseColor * (1f + colOffset);
    }
}