using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    
    public static LightManager Instance;
    [FormerlySerializedAs("IsLightOff")] public bool IsLightningFlash;
    
    private Coroutine brightenGlobalLightCo;
    private Coroutine lightningFlashCo;
    
    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 번개 치는 화면 효과 실행
    /// </summary>
    public void LightningFlash()
    {
        if (lightningFlashCo == null)
        {
            lightningFlashCo = StartCoroutine(LightningFlashRoutine());
        }
    }
    
    private IEnumerator LightningFlashRoutine()
    {
        float elapsedTime;
        float duration = 0.2f;
        IsLightningFlash = false;

        while (duration > 0)
        {
            duration -= Time.deltaTime * 10f;

            elapsedTime = 1f;

            while (elapsedTime >= 0f)
            {
                elapsedTime -= Time.deltaTime * 10f;
                globalLight.color = new Color(elapsedTime, elapsedTime, elapsedTime, 1);
                yield return null;
            }

            elapsedTime = 0;

            while (elapsedTime <= 1f)
            {
                elapsedTime += Time.deltaTime * 10f;
                globalLight.color = new Color(elapsedTime, elapsedTime, elapsedTime, 1);
            }
            
            yield return null;
        }

        globalLight.color = Color.white;
        IsLightningFlash = true;
         
        if (lightningFlashCo != null)
        {
            StopCoroutine(lightningFlashCo);
            lightningFlashCo = null;
        } 
    }
    
}
