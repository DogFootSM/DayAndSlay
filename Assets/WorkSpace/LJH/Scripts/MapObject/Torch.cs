using UnityEngine;
using UnityEngine.Rendering.Universal; // URP Light2D를 사용하기 위해 필요

public class Torch : MonoBehaviour
{
    private float slowSpeed = 1.2f;
    private float fastSpeed = 6f; 

    private float slowPowerX = 0.02f;
    private float fastPowerX = 0.01f; 

    private float slowPowerY = 0.06f;
    private float fastPowerY = 0.03f;  

    void Update()
    {
        float x = 1 + Mathf.Sin(Time.time * slowSpeed) * slowPowerX + Mathf.Sin(Time.time * fastSpeed) * fastPowerX;
        
        float y = 1 + Mathf.Sin(Time.time * slowSpeed * 1.1f) * slowPowerY + Mathf.Sin(Time.time * fastSpeed * 1.3f) * fastPowerY;
        
        transform.localScale = new Vector3(x, y, transform.localScale.z);
    }
}