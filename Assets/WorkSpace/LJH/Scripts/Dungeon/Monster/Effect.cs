using System;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void PlaySkill()
    {
        GetComponent<ParticleSystem>().Play();
    }

}