using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpkeepPopUp : MonoBehaviour
{
    private Animator animator;
    private AudioSource audio;
    private AudioClip clip;
    
    [SerializeField]private List<GameObject> Upkeeptexts;
    
    [SerializeField] private float animationDuration = 0.75f; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
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

        AllTextOn();
    }

    private IEnumerator CloseAnimationCompleteCoroutine()
    {
        AllTextOff();
        
        yield return new WaitForSeconds(animationDuration);

        gameObject.SetActive(false); 
    }

    private void AllTextOn()
    {
        foreach (GameObject text in Upkeeptexts)
        {
            text.SetActive(true);
        }
    }

    private void AllTextOff()
    {
        foreach (GameObject text in Upkeeptexts)
        {
            text.SetActive(false);
        }
    }
}