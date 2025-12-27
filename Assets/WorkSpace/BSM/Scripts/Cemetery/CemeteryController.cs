using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemeteryController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _deathAnimHash = Animator.StringToHash("Death");
    
    private void OnEnable()
    {
        _animator.Play(_deathAnimHash);
    }
}
