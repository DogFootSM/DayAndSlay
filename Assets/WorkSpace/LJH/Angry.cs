using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angry : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.Play("Angry");
    }
}