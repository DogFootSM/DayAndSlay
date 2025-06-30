using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField] List<GameObject> doors;

    public Tilemap mapTilemap;
    public Tilemap obstacleTilemap;

    public List<GameObject> GetDoorList()
    {
        return new(doors);
    }

    /// <summary>
    /// 문열기 함수
    /// </summary>
    /// <param name="doorNum1">첫번째 문</param>
    /// <param name="doorNum2">두번째 문</param>
    public void ActivateTheDoor(int doorNum1, int doorNum2)
    {
        doors[doorNum1].SetActive(true);
        doors[doorNum2].SetActive(true);
    }

    /// <summary>
    /// 문이 1개인 경우
    /// </summary>
    /// <param name="doorNum"></param>
    public void ActivateTheDoor(int doorNum)
    {
        doors[doorNum].SetActive(true);
    }

}
