using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffect : Effect
{
    [SerializeField] Animator animator;
    protected override void Tick() { }

    private void OnEnable()
    {
        base.OnEnable();
        Invoke("AnimationDelay", 0.5f);
    }

    void AnimationDelay()
    {
        animator.Play("Root");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("피해 입음");
        }
    }
}
