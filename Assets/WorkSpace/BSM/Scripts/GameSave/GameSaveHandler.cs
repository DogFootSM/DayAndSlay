using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject saveAskAlert;
    [SerializeField] private SavePromptUI savePromptUI;
    [SerializeField] private Image saveDimmedImage;
    [SerializeField] private GameObject blinkText;
    [SerializeField] private GameObject saveFailAlert;
    
    [Inject] private SaveManager saveManager;
    
    private Coroutine saveFadeCo;
    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    /// <summary>
    /// 저장 안내 얼럿 활성화
    /// /// History : 2025.12.17
    /// 작성자 : 이재호
    /// 세금내는날 && 아침에는 저장 예외 처리 추가
    /// </summary>
    public void OpenSaveAlert()
    {
        //세금내는 날 아침엔 침대에서 잘 수 없음
        if (IngameManager.instance.IsTaxDay() && DayManager.instance.GetDayOrNight() == DayAndNight.MORNING) return;
        
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
        
        saveDimmedImage.gameObject.SetActive(true);
        
        //FadeOut
        while (elapsedTime < hideProgress)
        {
            elapsedTime += Time.deltaTime;

            yield return null; 
            saveDimmedImage.color = new Color(0, 0, 0, elapsedTime);
        }
        
        blinkText.SetActive(true); 
        
        //SaveManager의 저장 로직 호출 및 저장 결과 반환
        bool saveSuccess = saveManager.GameDataSave();
        
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
        saveDimmedImage.gameObject.SetActive(false);
        
        //코루틴 상태 초기화
        if (saveFadeCo != null)
        {
            StopCoroutine(saveFadeCo);
            saveFadeCo = null;
        }
        
        //저장 실패 시 실패 얼럿 활성화
        if (!saveSuccess)
        {
            saveFailAlert.SetActive(true); 
        }
        
    } 
}
