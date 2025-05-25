using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    
    [Inject] private MapManager mapManager;
    [Inject] private DataManager dataManager;
    private GameObject playerInstance;
    private CharacterAnimatorController characterAnimatorController;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    private void Start()
    {
        PlayerSpawn();
        PlayerSpriteLoad();
    }

    /// <summary>
    /// 플레이어 캐릭터 생성
    /// </summary>
    private void PlayerSpawn()
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        characterAnimatorController = playerInstance.GetComponent<CharacterAnimatorController>();
        mapManager.ManChange(MapType.TOWN);
    }

    /// <summary>
    /// 플레이어 캐릭터 스프라이트 데이터 적용
    /// </summary>
    private void PlayerSpriteLoad()
    { 
        dataManager.LoadPresetData(characterAnimatorController); 
    }
}