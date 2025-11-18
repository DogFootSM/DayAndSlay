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
    private int currentValue = 0;

    [SerializeField] private Button taxPayButton;
    
    [SerializeField] private float animationDuration = 0.75f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
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