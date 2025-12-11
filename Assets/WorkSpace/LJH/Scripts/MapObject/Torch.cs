using UnityEngine;
using UnityEngine.Rendering.Universal; // URP Light2D를 사용하기 위해 필요

public class Torch : MonoBehaviour
{
    private Light2D light2D; // 제어할 2D Light 컴포넌트

    // 일렁임의 최소/최대 밝기
    [Header("밝기 설정")]
    [Tooltip("라이트가 가질 수 있는 최소 밝기")]
    public float minIntensity = 0.8f;
    [Tooltip("라이트가 가질 수 있는 최대 밝기")]
    public float maxIntensity = 1.2f;

    // 일렁임의 속도
    [Header("속도 설정")]
    [Tooltip("값이 높을수록 더 빠르게 일렁임")]
    public float flickerSpeed = 0.2f; 
    
    // 일렁임의 최소/최대 반지름
    [Header("반지름 설정")]
    public float minRadius = 1.8f;
    public float maxRadius = 2.2f;

    private float randomStart; // 무작위 시작 값을 위한 변수

    void Start()
    {
        // Light2D 컴포넌트 가져오기
        light2D = GetComponent<Light2D>(); 
        
        // Perlin Noise를 위한 무작위 시작값 설정
        randomStart = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (light2D == null) return;
        
        // Perlin Noise 계산 (동일 패턴)
        float noise = Mathf.PerlinNoise(randomStart, Time.time * flickerSpeed);

        // Intensity 적용
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        
        // Radius 적용
        light2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, noise);
    }
}