using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAfterImage : MonoBehaviour
{
    private float delete = 0.2f;
    private float deleteTimer;

    private void Start()
    {
        deleteTimer = delete;
    }

    void Update()
    {
        //일정 시간 경과 시 오브젝트 삭제
        if (deleteTimer > 0)
        {
            deleteTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject, 0.5f);            
        }
    }
}
