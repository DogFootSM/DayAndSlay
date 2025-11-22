using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class SpiritRush : MonoBehaviour
{
    private BansheeAI banshee;
    private bool isRushing;
    
    public float moveSpeed = 5f;
    public Direction dir;

    private void Start()
    {
        banshee = GetComponentInParent<BansheeAI>();
    }
    private void Update()
    {
        if (isRushing)
        {
            DirectionSetting();
        }
    }
    public void Rush()
    {
        isRushing = true;
    }

    public void SetRushDir(Direction dir)
    {
        this.dir = dir;
    }

    private void DirectionSetting()
    {
        Vector3 moveDirection = Vector3.zero;
        switch (dir)
        {
            case Direction.Up:
                moveDirection = Vector3.up;
                break;
            case Direction.Down:
                moveDirection = Vector3.down;
                break;
            case Direction.Right:
                moveDirection = Vector3.right;
                break;
            case Direction.Left:
                moveDirection = Vector3.left;
                break;
            default:
                break;
        }
    
        // Time.deltaTime을 곱해서 프레임과 상관없이 일정한 속도로 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void EndRush()
    {
        Debug.Log("영혼이 그만 질주해야함");
        isRushing = false;
        transform.position = banshee.transform.position;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 영혼에 맞음 (온콜리전엔터)!");
            // 기절, 데미지 처리...
        }
    }
}*/
