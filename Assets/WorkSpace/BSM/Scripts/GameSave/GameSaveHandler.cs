using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Zenject;

public class GameSaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject saveAskAlert;
    [SerializeField] private SavePromptUI savePromptUI;
    [SerializeField] private Image saveDimmedImage;
    [SerializeField] private GameObject blinkText;
    
    [Inject] private SaveManager saveManager;
    
    private Coroutine saveFadeCo;
    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    /// <summary>
    /// 저장 안내 얼럿 활성화
    /// </summary>
    public void OpenSaveAlert()
    { 
        saveAskAlert.SetActive(true); 
    }

    /// <summary>
    /// 게임 저장 프로세스 진행
    /// </summary>
    public void GameSaveProcess()
    {
        if (saveFadeCo == null)
        {
            saveFadeCo = StartCoroutine(GameSaveFadeRoutine());
        } 
    }
    
    /// <summary>
    /// 페이드 인, 아웃 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameSaveFadeRoutine()
    {
        float elapsedTime = 0;
        float hideProgress = 1.5f;
        
        //FadeOut
        while (elapsedTime < hideProgress)
        {
            elapsedTime += Time.deltaTime;

            yield return null; 
            saveDimmedImage.color = new Color(0, 0, 0, elapsedTime);
        }
        
        blinkText.SetActive(true); 
        
        //SaveManager의 저장 로직 호출
        saveManager.GameDataSave();
        //TODO: Save 호출 필요, Wait으로 기다리는게 아닌 async로 기다리기?
        yield return WaitCache.GetWait(2f);
        
        elapsedTime = 1.5f;
        
        //FadeIn
        while (elapsedTime >= 0f)
        {
            elapsedTime -= Time.deltaTime;

            yield return null;
            saveDimmedImage.color = new Color(0, 0, 0, elapsedTime);
        }

        //텍스트 블링크 비활성화
        blinkText.SetActive(false);
        
        //코루틴 상태 초기화
        if (saveFadeCo != null)
        {
            StopCoroutine(saveFadeCo);
            saveFadeCo = null;
        } 
    } 
}
