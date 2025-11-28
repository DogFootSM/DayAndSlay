using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class UpkeepPopUp : MonoBehaviour
{
    private Animator animator;
    private AudioSource audio;
    private AudioClip clip;

    [SerializeField][SerializedDictionary] private SerializedDictionary<string, GameObject> upkeepTextDict;
    //private int currentValue = 0;

    [SerializeField] private Button taxPayButton;
    
    [SerializeField] private float animationDuration = 0.75f;
    private RectMask2D mask;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
        mask = GetComponentInChildren<RectMask2D>();
    }

    private void Start()
    {
        IngameManager.instance.SetUpKeepText(upkeepTextDict);
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

    }

    private IEnumerator CloseAnimationCompleteCoroutine()
    {
        yield return new WaitForSeconds(animationDuration);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 마스크렌더링 / 애니메이션 이벤트로 호출
    /// </summary>
    /// <param name="padding">패딩값 left, bottom, right, up</param>
    public void RenderPadding(float padding)
    {
        mask.padding = new Vector4(padding, 0, padding, 0);
    }
}