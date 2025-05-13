using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    [Inject] private DataManager dataManager;

    private GameObject playerInstance;
    private CharacterAnimatorController characterAnimatorController;


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
    }

    /// <summary>
    /// 플레이어 캐릭터 스프라이트 데이터 적용
    /// </summary>
    private void PlayerSpriteLoad()
    {
        ProjectContext.Instance.Container.Inject(this);
        
        dataManager.LoadPresetData(characterAnimatorController); 
    }
}