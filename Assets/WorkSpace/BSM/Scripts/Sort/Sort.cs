using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sort
{ 
    private static Collider2D[] colliders;

    /// <summary>
    /// 플레이어아 가까운 위치 기준으로 몬스터 배열 정렬
    /// </summary>
    /// <param name="cols"></param>
    /// <param name="playerPosition"></param>
    public static void SortMonstersByNearest(Collider2D[] cols, Vector2 playerPosition)
    {
        colliders = new Collider2D[cols.Length];
        MergeSort(cols, 0, cols.Length -1, playerPosition);
    }

    /// <summary>
    /// 병합 정렬 재귀 호출
    /// </summary>
    /// <param name="cols">몬스터 원본 배열</param>
    /// <param name="start">비교 시작할 좌측 인덱스</param>
    /// <param name="end">비교 끝낼 우측 인덱스</param>
    /// <param name="playerPosition">플레이어의 위치</param>
    private static void MergeSort(Collider2D[] cols, int start, int end, Vector2 playerPosition)
    {
        if (start >= end) return;

        int middle = (start + end) / 2;
        MergeSort(cols, start, middle, playerPosition);
        MergeSort(cols, middle + 1, end, playerPosition);
        Merge(cols, start, middle, end, playerPosition);
    }

    /// <summary>
    /// 가까운 순으로 정렬 진행
    /// </summary>
    /// <param name="arr">몬스터 원본 배열</param>
    /// <param name="start">비교 시작할 좌측 인덱스</param>
    /// <param name="middle">분할할 중앙 인덱스</param>
    /// <param name="end">비교 끝낼 우측 인덱스</param>
    /// <param name="playerPosition">플레이어 위치</param>
    private static void Merge(Collider2D[] cols, int start, int middle, int end, Vector2 playerPosition)
    {
        int i = start;
        int j = middle + 1;
        int temp = 0;

        while (i <= middle && j <= end)
        {
            if (Vector2.Distance(playerPosition, cols[i].transform.position) <
                Vector2.Distance(playerPosition, cols[j].transform.position))
            {
                colliders[temp++] = cols[i++];
            }
            else
            {
                colliders[temp++] = cols[j++];
            }
        }

        while (i <= middle)
        {
            colliders[temp++] = cols[i++];
        }

        while (j <= end)
        {
            colliders[temp++] = cols[j++];
        }

        i = start;
        temp = 0;

        while (i <= end)
        {
            cols[i++] = colliders[temp++];
        }
        
    }
    
}
