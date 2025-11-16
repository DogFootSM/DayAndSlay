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

    [SerializeField][SerializedDictionary] private SerializedDictionary<string, GameObject> upkeepTextDict;
    [SerializeField] private TMP_InputField inputField;
    private int currentValue = 0;

    [SerializeField] private Button taxPayButton;
    
    [SerializeField] private float animationDuration = 0.75f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
    }

    private void OnEnable()
    {
        IngameManager.instance.SetUpKeepText(upkeepTextDict);
    }


    /// <summary>
    /// 입력된 값이 유저의 총 자산 넘는지 체크 > OnValueChanged로 호출
    /// </summary>
    public void CheckInputDept()
    {
        if (!int.TryParse(inputField.text, out int temp)) return;
        //Todo : 300을 유저의 잔고로 바꿔야함

        if (int.Parse(inputField.text) > 300000)
        {
            inputField.text = 300000.ToString();
        }
        
        currentValue = int.Parse(inputField.text);
    }

    /// <summary>
    /// 세금 지불
    /// </summary>
    public void PayTax()
    {
        Debug.Log(currentValue);
        IngameManager.instance.PayTax(currentValue);
        IngameManager.instance.SetUpKeepText(upkeepTextDict);
        IngameManager.instance.PayTaxResult(true);
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
