using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private string effectName;
    private void OnEnable()
    {
        GetComponent<Animator>().Play(effectName);
    }

}
