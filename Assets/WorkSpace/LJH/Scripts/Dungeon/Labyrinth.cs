using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    private MinoMethod mino;
    private void Start()
    {
        StartCoroutine(Setcoroutine());
    }

    private IEnumerator Setcoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        mino = GetComponentInParent<BossMonsterSpawner>().GetBossMonster(0).GetComponent<MinoMethod>();

        mino.SetLabyrinth(this);
        
        gameObject.SetActive(false);
    }
}
