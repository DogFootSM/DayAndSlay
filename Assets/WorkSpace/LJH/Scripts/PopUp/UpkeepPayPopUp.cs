using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpkeepPayPopUp : MonoBehaviour
{
    private Animator animator;
    private AudioSource audio;
    private AudioClip clip;
    
    private IngameManager ingameManager;

    [SerializeField][SerializedDictionary] private SerializedDictionary<string, GameObject> upkeepTextDict;
    [SerializeField] private TMP_InputField inputField;
    private int currentValue = 0;

    [SerializeField] private Button taxPayButton;
    [SerializeField] private GameObject alertText;
    private bool isAlert = false;
    
    [SerializeField] private float animationDuration = 0.75f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        clip = audio.clip;

        ingameManager = IngameManager.instance;
    }

    private void OnEnable()
    {
        ingameManager.SetUpKeepText(upkeepTextDict);
    }


    /// <summary>
    /// 입력된 값이 유저의 총 자산 넘는지 체크 > OnValueChanged로 호출
    /// </summary>
    public void CheckInputDept()
    {
        if (!int.TryParse(inputField.text, out int temp)) return;
        //Todo : 300을 유저의 잔고로 바꿔야함

        if (int.Parse(inputField.text) > ingameManager.GetCurrentGold())
        {
            inputField.text = ingameManager.GetCurrentGold().ToString();

            if (!isAlert)
            {
                isAlert = true;
                StartCoroutine(AlertMessageCoroutine());
            }
        }
        
        currentValue = int.Parse(inputField.text);
    }

    /// <summary>
    /// 경고 문구 활성화/비활성화
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertMessageCoroutine()
    {
        alertText.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        
        alertText.SetActive(false);
        isAlert = false;
    }

    /// <summary>
    /// 세금 지불
    /// </summary>
    public void PayTax()
    {
        ingameManager.SetGold(-currentValue);
        ingameManager.PayTax(currentValue);
        ingameManager.SetUpKeepText(upkeepTextDict);
        //ingameManager.PayTaxResult(true);
    }

    public void PlayOpen()
    {
        audio.PlayOneShot(clip);
        animator.Play("UpkeepOpen");
        StartCoroutine(OpenAnimationCompleteCoroutine());
    }

    public void PlayClose()
    {
        audio.PlayOneShot(clip);
        animator.Play("UpkeepClose");
        StartCoroutine(CloseAnimationCompleteCoroutine());
    }


    private IEnumerator OpenAnimationCompleteCoroutine()
    {
        yield return new WaitForSeconds(animationDuration);

        AllTextActive(true);
    }

    private IEnumerator CloseAnimationCompleteCoroutine()
    {
        AllTextActive(false);

        yield return new WaitForSeconds(animationDuration);

        gameObject.SetActive(false);
    }

    private void AllTextActive(bool isActive)
    {
        foreach (var text in upkeepTextDict)
        {
            text.Value.SetActive(isActive);
        }
    }
}
